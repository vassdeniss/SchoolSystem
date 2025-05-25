document.addEventListener('DOMContentLoaded', () => {
  // Sidebar
  const burger = document.getElementById('burgerBtn');
  const sideMenu = document.getElementById('sideMenu');
  burger.addEventListener('click', () => sideMenu.classList.toggle('open'));

  // Edit page
  const editBtn = document.getElementById('editPageBtn');
  const addForm = document.getElementById('addForm');
  const enrollForm = document.getElementById('enrollForm');
  let editMode = false;

  // Enable/disable form inputs
  function toggleFormInputs(enable) {
    Array.from(addForm.elements).forEach(el => {
      if (el.tagName !== 'BUTTON') el.disabled = !enable;
    });
  }

  // Toggle page editing
  editBtn.addEventListener('click', () => {
    editMode = !editMode;
    editBtn.textContent = editMode ? 'Запази' : 'Редактирай';
    document.body.classList.toggle('page-editing', editMode);
    toggleFormInputs(editMode);
    renderTable();
  });

  // Mode buttons
  const modeEnroll = document.getElementById('modeEnroll');
  const modeUnenroll = document.getElementById('modeUnenroll');
  let mode = 'enroll';

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
  // стартово
  modeEnroll.click();

  // Demo данни
  let students = [
    { name: 'Иван Иванов', cls: '8', user: 'ivan8', pass: '••••' },
    { name: 'Петър Петров', cls: '9', user: 'petar9', pass: '••••' }
  ];
  const tableBody = document.querySelector('#stuTable tbody');

  // Рендер на таблицата
  function renderTable() {
    tableBody.innerHTML = '';
    students.forEach((s, i) => {
      const tr = document.createElement('tr');
      tr.innerHTML = `
        <td>${s.name}</td>
        <td>${s.cls}</td>
        <td>${s.user}</td>
        <td>${s.pass}</td>
        <td>
          <button class="action-btn edit"   data-i="${i}" title="Редактирай">✏️</button>
          <button class="action-btn delete" data-i="${i}" title="Изтрий">🗑️</button>
        </td>`;
      tableBody.appendChild(tr);

      // Ако editMode, правим td-та contenteditable
      if (editMode) {
        Array.from(tr.children).slice(0, 4).forEach(td => {
          td.contentEditable = 'true';
          td.classList.add('editable');
        });
      } else {
        Array.from(tr.children).slice(0, 4).forEach(td => {
          td.contentEditable = 'false';
          td.classList.remove('editable');
        });
      }

      // Save from row (само когато editMode)
      tr.querySelector('.edit').onclick = () => {
        if (!editMode) return;
        const idx = i;
        const cells = tr.querySelectorAll('td');
        students[idx] = {
          name: cells[0].textContent.trim(),
          cls: cells[1].textContent.trim(),
          user: cells[2].textContent.trim(),
          pass: cells[3].textContent.trim()
        };
        renderTable();
      };

      // Delete
      tr.querySelector('.delete').onclick = () => {
        if (!editMode && mode === 'unenroll') {
          // в режим „отпиши“ изтриваме маркираните 
        }
        if (confirm('Сигурни ли сте, че искате да изтриете този запис?')) {
          students.splice(i, 1);
          renderTable();
        }
      };

      // В режим „отпиши“ добавяме чекбокс
      if (mode === 'unenroll') {
        const cb = document.createElement('input');
        cb.type = 'checkbox';
        cb.dataset.i = i;
        const tdCb = document.createElement('td');
        tdCb.appendChild(cb);
        tr.insertBefore(tdCb, tr.lastElementChild);
      }
    });
  }

  // Добавяне на нов ученик
  addForm.addEventListener('submit', e => {
    e.preventDefault();
    if (!editMode) return;
    const nm = document.getElementById('newName').value.trim();
    const cl = document.getElementById('newClass').value;
    const us = document.getElementById('newUser').value.trim();
    const pw = document.getElementById('newPass').value.trim();
    if (nm && cl && us && pw) {
      students.push({ name: nm, cls: cl, user: us, pass: pw });
      addForm.reset();
      renderTable();
    }
  });

  renderTable();
});
