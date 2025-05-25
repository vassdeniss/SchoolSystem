document.addEventListener('DOMContentLoaded', () => {
  // Sidebar toggle
  const burger = document.getElementById('burgerBtn');
  const side = document.getElementById('sideMenu');
  burger.addEventListener('click', () => side.classList.toggle('open'));

  // Elements
  const modeAdd = document.getElementById('modeAdd');
  const modeRemove = document.getElementById('modeRemove');
  const formSec = document.getElementById('addFormSection');
  const addForm = document.getElementById('addTeacherForm');
  const tbody = document.querySelector('#teachersTable tbody');

  let teachers = [
    {
      name: 'Иван Иванов',
      subject: 'Математика',
      classes: ['8', '9'],
      status: 'active',
      user: 'iivanov',
      pass: '••••'
    }
  ];
  let mode = 'add';

  function render() {
    tbody.innerHTML = '';
    teachers.forEach((t, i) => {
      const tr = document.createElement('tr');
      let clsList = t.classes.map(c => c + ' клас').join(', ');
      tr.innerHTML = `
        <td>${mode === 'remove' ? `<input type="checkbox" data-i="${i}">` : ''}</td>
        <td>${t.name}</td>
        <td>${t.subject}</td>
        <td>${clsList}</td>
        <td>${t.status === 'active' ? 'Активен' : 'Неактивен'}</td>
        <td>${t.user}</td>
        <td>${t.pass}</td>
        <td>
          <button class="action-btn edit"   data-i="${i}" title="Редактирай">✏️</button>
          <button class="action-btn delete" data-i="${i}" title="Изтрий">🗑️</button>
        </td>`;
      tbody.appendChild(tr);

      // handlers
      tr.querySelector('.edit').onclick = () => {
        const idx = +tr.querySelector('.edit').dataset.i;
        const te = teachers[idx];
        const nn = prompt('Име:', te.name);
        const sbj = prompt('Предмет:', te.subject);
        // класове
        const cls = prompt('Класове (разделени с ,):', te.classes.join(','));
        const st = prompt('Статус (active/inactive):', te.status);
        const us = prompt('Потребител:', te.user);
        const pw = prompt('Парола:', te.pass);
        if (nn && sbj && cls && st && us && pw) {
          teachers[idx] = {
            name: nn,
            subject: sbj,
            classes: cls.split(',').map(s => s.trim()),
            status: st,
            user: us,
            pass: pw
          };
          render();
        }
      };
      tr.querySelector('.delete').onclick = () => {
        if (confirm('Изтриване?')) {
          teachers.splice(i, 1);
          render();
        }
      };
    });
  }

  // Modes
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

  // Add form
  addForm.onsubmit = e => {
    e.preventDefault();
    const nm = document.getElementById('teacherName').value.trim();
    const sb = document.getElementById('teacherSubject').value.trim();
    const ch = Array.from(document.querySelectorAll('.checkbox-group input:checked'))
      .map(el => el.value);
    const st = document.getElementById('teacherStatus').value;
    const us = document.getElementById('teacherUser').value.trim();
    const pw = document.getElementById('teacherPass').value.trim();
    teachers.push({ name: nm, subject: sb, classes: ch, status: st, user: us, pass: pw });
    addForm.reset();
    render();
  };
});
