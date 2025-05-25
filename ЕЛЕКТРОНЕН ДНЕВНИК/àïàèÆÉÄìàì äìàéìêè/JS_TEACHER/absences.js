document.addEventListener('DOMContentLoaded', () => {
  //  Toggle sidebar
  const menuBtn = document.querySelector('.menu-btn');
  const sideMenu = document.getElementById('sideMenu');
  menuBtn.addEventListener('click', () => {
    sideMenu.classList.toggle('open');
  });

  // Демонстрационни данни
  const classes = {
    classA: ['Иван Иванов', 'Петър Петров', 'Мария Георгиева'],
    classB: ['Георги Георгиев', 'Нели Ненова', 'Калин Калинов'],
    classC: ['Анна Андонова', 'Борис Борисов', 'Стефка Стефанова']
  };
  let attendanceRecords = []; // { student, date, present }

  const tbody = document.querySelector('#abs-table tbody');
  const classSelect = document.getElementById('class-select');

  // рендиране на таблицата
  function renderTable() {
    tbody.innerHTML = '';
    const students = classes[classSelect.value];
    const today = new Date().toISOString().slice(0, 10);

    students.forEach(name => {
      // намираме рекорд за днешната дата
      const rec = attendanceRecords.find(r => r.student === name && r.date === today);
      const present = rec ? rec.present : true;
      const tr = document.createElement('tr');
      tr.innerHTML = `
        <td>${name}</td>
        <td>${today}</td>
        <td><input type="checkbox" ${present ? 'checked' : ''} data-st="${name}"></td>
      `;
      tbody.appendChild(tr);
    });

    // слушатели за чекбоксове
    tbody.querySelectorAll('input[type="checkbox"]').forEach(cb => {
      cb.addEventListener('change', () => {
        const student = cb.dataset.st;
        const date = today;
        // махаме стар рекорд, ако има
        attendanceRecords = attendanceRecords.filter(r => !(r.student === student && r.date === date));
        attendanceRecords.push({ student, date, present: cb.checked });
        renderChart(); // обновяваме диаграмата
      });
    });
  }

  // диаграма (Chart.js)
  function renderChart() {
    const ctx = document.getElementById('absChart').getContext('2d');
    const students = classes[classSelect.value];
    const today = new Date().toISOString().slice(0, 10);
    const data = students.map(name => {
      const rec = attendanceRecords.find(r => r.student === name && r.date === today);
      return rec ? (rec.present ? 1 : 0) : 1;
    });

    // махаме старата диаграма, ако има
    if (window.absenceChart) {
      window.absenceChart.destroy();
    }

    window.absenceChart = new Chart(ctx, {
      type: 'bar',
      data: {
        labels: students,
        datasets: [{
          label: 'Присъствие (1=да, 0=не)',
          data,
        }]
      },
      options: {
        scales: {
          y: { min: 0, max: 1, ticks: { stepSize: 1 } }
        }
      }
    });
  }

  // при смяна на клас
  classSelect.addEventListener('change', () => {
    renderTable();
    renderChart();
  });

  // първоначално
  renderTable();
  renderChart();
});
