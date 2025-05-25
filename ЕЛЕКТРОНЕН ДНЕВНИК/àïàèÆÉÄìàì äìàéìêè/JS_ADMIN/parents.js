document.addEventListener('DOMContentLoaded', () => {
  // Sidebar toggle
  const burger = document.getElementById('burgerBtn');
  const sideMenu = document.getElementById('sideMenu');
  burger.addEventListener('click', () => sideMenu.classList.toggle('open'));

  // Edit-page button
  const editBtn = document.getElementById('editPageBtn');
  let editMode = false;
  function toggleInputs(enable) {
    // FORM
    document.querySelectorAll('#addParentForm input, #addParentForm select, #addParentForm button')
      .forEach(el => el.disabled = !enable);
    // TABLE
    document.querySelectorAll('#parentsTable tbody td').forEach(td => {
      td.contentEditable = enable;
      td.classList.toggle('editable', enable);
    });
  }
  editBtn.addEventListener('click', () => {
    editMode = !editMode;
    editBtn.textContent = editMode ? 'Запази' : 'Редактирай';
    toggleInputs(editMode);
  });

  // Mode buttons
  const modeAdd = document.getElementById('modeAdd');
  const modeRemove = document.getElementById('modeRemove');
  const formSec = document.getElementById('addFormSection');
  let mode = 'add';
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

  // Demo данни
  let parents = [
    { name: 'Георги Иванов', child: 'Мария Георгиева', cls: '8', user: 'givanov', pass: '••••' }
  ];

  const tbody = document.querySelector('#parentsTable tbody');

  // Render
  function render() {
    tbody.innerHTML = '';
    parents.forEach((p, i) => {
      const tr = document.createElement('tr');
      tr.innerHTML = `
        <td>${p.name}</td>
        <td>${p.child}</td>
        <td>${p.cls}</td>
        <td>${p.user}</td>
        <td>${p.pass}</td>
        <td>
          <button class="action-btn edit"   data-i="${i}" title="Редактирай">✏️</button>
          <button class="action-btn delete" data-i="${i}" title="Изтрий">🗑️</button>
        </td>`;
      // ако е режим „отпиши“
      if (mode === 'remove') {
        const cb = document.createElement('input');
        cb.type = 'checkbox'; cb.dataset.i = i;
        tr.insertBefore(cb, tr.firstChild);
      }
      tbody.appendChild(tr);

      // бутон „Редактирай“ за единствен ред
      const editBtn = tr.querySelector('.edit');
      editBtn.addEventListener('click', () => {
        const tds = tr.querySelectorAll('td');
        const isEditing = tr.classList.toggle('row-editing');
        tds.forEach((td, idx) => {
          if (idx < 5) {
            td.contentEditable = isEditing;
            td.classList.toggle('editable', isEditing);
          }
        });
        editBtn.textContent = isEditing ? '💾' : '✏️';
        editBtn.title = isEditing ? 'Запази този ред' : 'Редактирай';
        if (!isEditing) {
          // при запазване: вземаме новите стойности и презаписваме масива
          const [name, child, cls, user, pass] = Array.from(tds).slice(0, 5).map(td => td.textContent.trim());
          parents[i] = { name, child, cls, user, pass };
          render(); // презареждаме за да изчистим contenteditable и обновим бутона
        }
      });

      // бутон „Изтрий“
      tr.querySelector('.delete').addEventListener('click', () => {
        if (confirm('Изтриване?')) {
          parents.splice(i, 1);
          render();
        }
      });
    });
   
  }

  // Add new
  document.getElementById('addParentForm').onsubmit = e => {
    e.preventDefault();
    if (!editMode) return;
    const nn = document.getElementById('parentName').value.trim();
    const ch = document.getElementById('childName').value.trim();
    const cl = document.getElementById('childClass').value;
    const us = document.getElementById('parentUser').value.trim();
    const pw = document.getElementById('parentPass').value.trim();
    if (nn && ch && cl && us && pw) {
      parents.push({ name: nn, child: ch, cls: cl, user: us, pass: pw });
      e.target.reset();
      render();
    }
  };

  // Initial
  render();
});
