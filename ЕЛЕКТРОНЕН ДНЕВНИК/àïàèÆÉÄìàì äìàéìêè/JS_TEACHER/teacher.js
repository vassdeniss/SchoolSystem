document.addEventListener('DOMContentLoaded', () => {
  //  Toggle sidebar
  const menuBtn = document.querySelector('.menu-btn');
  const sideMenu = document.getElementById('sideMenu');
  menuBtn.addEventListener('click', () => {
    sideMenu.classList.toggle('open');
  });

  //  Данни за класове и ученици
  const classes = {
    '10A': ['Иван Иванов', 'Петър Петров', 'Мария Георгиева'],
    '10B': ['Георги Георгиев', 'Анна Димитрова'],
    '11A': ['Калин Колев', 'Елена Иванова']
  };
  let grades = [
    { cls: '10A', student: 'Иван Иванов', date: '2025-05-01', grade: '5' },
    { cls: '10A', student: 'Петър Петров', date: '2025-05-02', grade: '6' },
    { cls: '10B', student: 'Георги Георгиев', date: '2025-05-03', grade: '4' }
  ];
  let attendance = [];

  const classSelect = document.getElementById('class-select');
  const gradesTbody = document.querySelector('#grades-table tbody');
  const attTbody = document.querySelector('#att-table tbody');
  const addGradeBtn = document.getElementById('add-grade-btn');

  // Рендер функции
  function renderGrades() {
    const cls = classSelect.value;
    gradesTbody.innerHTML = '';
    grades.filter(g => g.cls === cls).forEach((g, i) => {
      const tr = document.createElement('tr');
      tr.innerHTML = `
        <td>${g.student}</td>
        <td contenteditable class="editable" data-field="date">${g.date}</td>
        <td contenteditable class="editable" data-field="grade">${g.grade}</td>
        <td class="actions">
          <button class="save" data-i="${i}" title="Запази промени">💾</button>
          <button class="delete" data-i="${i}" title="Изтрий оценка">🗑️</button>
        </td>`;
      gradesTbody.appendChild(tr);
    });
    // Save / Delete
    document.querySelectorAll('.save').forEach(btn => {
      btn.onclick = () => {
        const i = +btn.dataset.i;
        const tr = btn.closest('tr');
        grades[i].date = tr.querySelector('[data-field="date"]').textContent;
        grades[i].grade = tr.querySelector('[data-field="grade"]').textContent;
        renderGrades();
      };
    });
    document.querySelectorAll('.delete').forEach(btn => {
      btn.onclick = () => {
        grades.splice(+btn.dataset.i, 1);
        renderGrades();
      };
    });
  }

  function renderAttendance() {
    const cls = classSelect.value;
    const today = new Date().toISOString().slice(0, 10);
    attTbody.innerHTML = '';
    (classes[cls] || []).forEach(student => {
      const rec = attendance.find(a => a.cls === cls && a.student === student && a.date === today);
      const pres = rec ? rec.present : true;
      const tr = document.createElement('tr');
      tr.innerHTML = `
        <td>${student}</td>
        <td>${today}</td>
        <td><input type="checkbox" ${pres ? 'checked' : ''} data-st="${student}"></td>`;
      attTbody.appendChild(tr);
    });
    attTbody.querySelectorAll('input[type="checkbox"]').forEach(cb => {
      cb.onchange = () => {
        const student = cb.dataset.st;
        // премахваме предишната за днес, след това добавяме
        attendance = attendance.filter(a =>
          !(a.cls === classSelect.value && a.student === student && a.date === today)
        );
        attendance.push({
          cls: classSelect.value,
          student,
          date: today,
          present: cb.checked
        });
      };
    });
  }

  // При смяна на клас – рефреш на таблиците
  classSelect.addEventListener('change', () => {
    renderGrades();
    renderAttendance();
  });

  // „Добави оценка“
  addGradeBtn.onclick = () => {
    const cls = classSelect.value;
    const student = prompt('Ученик:', classes[cls][0] || '');
    const date = prompt('Дата (YYYY-MM-DD):', new Date().toISOString().slice(0, 10));
    const grade = prompt('Оценка:', '5');
    if (student && date && grade) {
      grades.push({ cls, student, date, grade });
      renderGrades();
    }
  };

  // първоначално
  renderGrades();
  renderAttendance();
});
