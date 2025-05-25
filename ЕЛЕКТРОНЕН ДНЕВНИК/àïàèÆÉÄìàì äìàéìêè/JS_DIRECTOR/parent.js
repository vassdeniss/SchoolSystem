document.addEventListener('DOMContentLoaded', () => {
  // sidebar toggle
  const burger = document.getElementById('burgerBtn');
  const side = document.getElementById('sideMenu');
  burger.addEventListener('click', () => side.classList.toggle('open'));

  // елементи
  const modeAdd = document.getElementById('modeAdd');
  const modeRemove = document.getElementById('modeRemove');
  const formSec = document.getElementById('addFormSection');
  const addForm = document.getElementById('addParentForm');
  const tbody = document.querySelector('#parentsTable tbody');

  let parents = [
    { name: 'Георги Иванов', child: 'Мария Георгиева', cls: '8', user: 'givanov', pass: '••••' }
  ];
  let mode = 'add';

  function render() {
    tbody.innerHTML = '';
    parents.forEach((p, i) => {
      const tr = document.createElement('tr');
      tr.innerHTML = `
        <td>${p.name}</td>
        <td>${p.child}</td>
        <td>${p.cls} клас</td>
        <td>${p.user}</td>
        <td>${p.pass}</td>
        <td>
          <button class="action-btn edit"   data-i="${i}" title="Редактирай">✏️</button>
          <button class="action-btn delete" data-i="${i}" title="Изтрий">🗑️</button>
        </td>`;
      // ако режим remove, добавя чекбокс
      if (mode === 'remove') {
        const cb = document.createElement('input');
        cb.type = 'checkbox'; cb.dataset.i = i;
        tr.insertBefore(cb, tr.firstChild);
      }
      tbody.appendChild(tr);

      // handlers
      tr.querySelector('.edit').onclick = () => {
        const idx = +tr.querySelector('.edit').dataset.i;
        const p0 = parents[idx];
        const nn = prompt('Име:', p0.name);
        const ch = prompt('Родител на:', p0.child);
        const cl = prompt('Клас:', p0.cls);
        const us = prompt('Потребител:', p0.user);
        const pw = prompt('Парола:', p0.pass);
        if (nn && ch && cl && us && pw) {
          parents[idx] = { name: nn, child: ch, cls: cl, user: us, pass: pw };
          render();
        }
      };
      tr.querySelector('.delete').onclick = () => {
        if (confirm('Изтриване?')) {
          parents.splice(i, 1);
          render();
        }
      };
    });
  }

  // превключватели
  modeAdd.onclick = () => {
    mode = 'add';
    modeAdd.classList.add('active');
    modeRemove.classList.remove('active');
    formSec.style.display = 'block';
    render();
  };
  modeRemove.onclick = () => {
    mode = 'remove';
    modeRemove.classList.add('active');
    modeAdd.classList.remove('active');
    formSec.style.display = 'none';
    render();
  };
  modeAdd.click();

  // формата
  addForm.onsubmit = e => {
    e.preventDefault();
    const nm = document.getElementById('parentName').value.trim();
    const ch = document.getElementById('childName').value.trim();
    const cl = document.getElementById('childClass').value;
    const us = document.getElementById('parentUser').value.trim();
    const pw = document.getElementById('parentPass').value.trim();
    parents.push({ name: nm, child: ch, cls: cl, user: us, pass: pw });
    addForm.reset();
    render();
  };
});
