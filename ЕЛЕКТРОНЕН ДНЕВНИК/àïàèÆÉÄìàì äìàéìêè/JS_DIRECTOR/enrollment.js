document.addEventListener('DOMContentLoaded', () => {
  // Sidebar toggle
  const burger = document.getElementById('burgerBtn');
  const side = document.getElementById('sideMenu');
  burger.addEventListener('click', () => side.classList.toggle('open'));

  // Елементи
  const modeEnroll = document.getElementById('modeEnroll');
  const modeUnenroll = document.getElementById('modeUnenroll');
  const enrollForm = document.getElementById('enrollForm');
  const addForm = document.getElementById('addForm');
  const tableBody = document.querySelector('#stuTable tbody');

  // Демонстрационни данни
  let students = [
    { name: 'Иван Иванов', cls: '8', user: 'ivan8', pass: '••••' },
    { name: 'Петър Петров', cls: '9', user: 'petar9', pass: '••••' }
  ];

  let mode = 'enroll'; // или 'unenroll'

  function renderTable() {
    tableBody.innerHTML = '';
    students.forEach((s, i) => {
      const tr = document.createElement('tr');
      tr.innerHTML = `
        <td>${s.name}</td>
        <td>${s.cls} клас</td>
        <td>${s.user}</td>
        <td>${s.pass}</td>
        <td>
          <button class="action-btn edit"   data-i="${i}" title="Редактирай">✏️</button>
          <button class="action-btn delete" data-i="${i}" title="Изтрий">🗑️</button>
        </td>`;
      tableBody.appendChild(tr);

      // Редактиране
      tr.querySelector('.edit').onclick = () => {
        const idx = tr.querySelector('.edit').dataset.i;
        const s0 = students[idx];
        const nn = prompt('Име:', s0.name);
        const nc = prompt('Клас:', s0.cls);
        const nu = prompt('Потребител:', s0.user);
        const np = prompt('Парола:', s0.pass);
        if (nn && nc && nu && np) {
          students[idx] = { name: nn, cls: nc, user: nu, pass: np };
          renderTable();
        }
      };
      // Изтриване
      tr.querySelector('.delete').onclick = () => {
        if (confirm('Сигурни ли сте?')) {
          students.splice(i, 1);
          renderTable();
        }
      };
      // При режим "отпиши" - добавяме чекбокс
      if (mode === 'unenroll') {
        const cb = document.createElement('input');
        cb.type = 'checkbox';
        cb.dataset.i = i;
        const td = document.createElement('td');
        td.appendChild(cb);

        tr.insertBefore(td, tr.lastElementChild);
      }
    });
  }

  // Превключване режими
  modeEnroll.addEventListener('click', () => {
    mode = 'enroll';
    modeEnroll.classList.add('active');
    modeUnenroll.classList.remove('active');
    enrollForm.style.display = 'block';
    renderTable();
  });
  modeUnenroll.addEventListener('click', () => {
    mode = 'unenroll';
    modeUnenroll.classList.add('active');
    modeEnroll.classList.remove('active');
    enrollForm.style.display = 'none';
    renderTable();
  });

  // Първоначално
  modeEnroll.click();

  // Добавяне ученик
  addForm.addEventListener('submit', e => {
    e.preventDefault();
    const nm = document.getElementById('newName').value.trim();
    const cl = document.getElementById('newClass').value;
    const us = document.getElementById('newUser').value.trim();
    const pw = document.getElementById('newPass').value.trim();
    students.push({ name: nm, cls: cl, user: us, pass: pw });
    addForm.reset();
    renderTable();
  });
});
