document.addEventListener('DOMContentLoaded', () => {
  const menuBtn = document.getElementById('menu-btn'),
    sideMenu = document.getElementById('sideMenu'),
    editBtn = document.getElementById('editBtn'),
    saveBtn = document.getElementById('saveBtn'),
    form = document.getElementById('teacherForm'),
    tbody = document.getElementById('teachersTbody');

  // Sidebar toggle
  menuBtn.addEventListener('click', () => sideMenu.classList.toggle('open'));

  // Demo data
  let teachers = [
    { name: 'Иван Иванов', subj: 'Математика', classes: ['8', '9'], user: 'ivan8', pass: '****' },
    { name: 'Петър Петров', subj: 'История', classes: ['10'], user: 'petar10', pass: '****' }
  ];

  // Render table
  function render() {
    tbody.innerHTML = '';
    teachers.forEach((t, i) => {
      const tr = document.createElement('tr');
      tr.innerHTML = `
        <td ${form.classList.contains('editing') ? 'contenteditable' : ''}>${t.name}</td>
        <td ${form.classList.contains('editing') ? 'contenteditable' : ''}>${t.subj}</td>
        <td ${form.classList.contains('editing') ? 'contenteditable' : ''}>${t.classes.join(', ')} клас</td>
        <td ${form.classList.contains('editing') ? 'contenteditable' : ''}>${t.user}</td>
        <td ${form.classList.contains('editing') ? 'contenteditable' : ''}>${t.pass}</td>
        <td class="actions">
          <button class="save"   data-i="${i}" title="Запази">💾</button>
          <button class="delete" data-i="${i}" title="Изтрий">🗑️</button>
        </td>`;
      tbody.appendChild(tr);
    });

    tbody.querySelectorAll('.save').forEach(btn => {
      btn.onclick = () => {
        const i = +btn.dataset.i;
        const tr = btn.closest('tr');
        const cells = tr.querySelectorAll('td[contenteditable]');
        teachers[i].name = cells[0].textContent.trim();
        teachers[i].subj = cells[1].textContent.trim();
        teachers[i].classes = cells[2].textContent.trim().replace(' клас', '').split(',').map(s => s.trim());
        teachers[i].user = cells[3].textContent.trim();
        teachers[i].pass = cells[4].textContent.trim();
        render();
      };
    });
    tbody.querySelectorAll('.delete').forEach(btn => {
      btn.onclick = () => {
        teachers.splice(+btn.dataset.i, 1);
        render();
      };
    });
  }

  // Toggle edit mode
  editBtn.addEventListener('click', () => {
    form.classList.toggle('editing');
    const inputs = form.querySelectorAll('input, #tClasses input');
    inputs.forEach(i => i.disabled = !i.disabled);
    saveBtn.disabled = !saveBtn.disabled;
    editBtn.textContent = form.classList.contains('editing') ? 'Запази страница' : 'Редактирай';
    render();
  });

  // Add new teacher
  saveBtn.addEventListener('click', () => {
    const nameEl = document.getElementById('tName'),
      subjEl = document.getElementById('tSubject'),
      checked = form.querySelectorAll('#tClasses input:checked'),
      userEl = document.getElementById('tUser'),
      passEl = document.getElementById('tPass');

    const name = nameEl.value.trim(),
      subj = subjEl.value.trim(),
      classes = Array.from(checked).map(i => i.value),
      user = userEl.value.trim(),
      pass = passEl.value.trim();

    if (!name || !subj || !classes.length || !user || !pass) {
      return alert('Попълнете всички полета!');
    }
    teachers.push({ name, subj, classes, user, pass });
    render();
    form.reset();
  });

  render();
});
