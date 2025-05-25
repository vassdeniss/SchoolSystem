document.addEventListener('DOMContentLoaded', () => {
  //  Toggle sidebar
  const menuBtn = document.getElementById('menuBtn');
  const sideMenu = document.getElementById('sideMenu');
  menuBtn.addEventListener('click', () => {
    sideMenu.classList.toggle('open');
  });

  //  Демонстрационни данни
  const upcoming = [
    { subject: 'Математика', type: 'Тест', date: '2025-05-10', time: '10:00', loc: 'Ауд.5', status: 'Предстои' },
    { subject: 'Български', type: 'Контролно', date: '2025-05-12', time: '12:00', loc: 'Онлайн', status: 'Предстои' },
    { subject: 'Физика', type: 'Лабораторна', date: '2025-05-15', time: '09:00', loc: 'Лаб.1', status: 'Предстои' }
  ];
  const archive = [
    { subject: 'Математика', type: 'Тест', date: '2025-04-20', grade: 'Очаква оценка' },
    { subject: 'Български', type: 'Тест', date: '2025-04-22', grade: 'Оценен' },
    { subject: 'Физика', type: 'Контролно', date: '2025-04-25', grade: 'Оценен' }
  ];

  //  Елементи
  const upTbody = document.querySelector('#upcomingTable tbody');
  const archTbody = document.querySelector('#archiveTable tbody');
  const subjFilter = document.getElementById('subjectFilter');
  const searchInp = document.getElementById('testSearch');
  const addUpBtn = document.getElementById('addUpcomingBtn');
  const scrollBtn = document.getElementById('scrollTopBtn');
  let chartInst;

  //  Рендер на таблици и графика
  function renderTables() {
    const subj = subjFilter.value.toLowerCase();
    const term = searchInp.value.toLowerCase();

    // Предстоящи
    upTbody.innerHTML = '';
    upcoming
      .filter(t => subj === 'all' || t.subject.toLowerCase() === subj)
      .filter(t => Object.values(t).some(v => String(v).toLowerCase().includes(term)))
      .forEach(t => {
        upTbody.insertAdjacentHTML('beforeend', `
          <tr>
            <td>${t.subject}</td>
            <td>${t.type}</td>
            <td>${t.date}</td>
            <td>${t.time}</td>
            <td>${t.loc}</td>
            <td>${t.status}</td>
          </tr>
        `);
      });

    // Архив
    archTbody.innerHTML = '';
    archive
      .filter(t => subj === 'all' || t.subject.toLowerCase() === subj)
      .filter(t => Object.values(t).some(v => String(v).toLowerCase().includes(term)))
      .forEach(t => {
        archTbody.insertAdjacentHTML('beforeend', `
          <tr>
            <td>${t.subject}</td>
            <td>${t.type}</td>
            <td>${t.date}</td>
            <td>${t.grade}</td>
          </tr>
        `);
      });

    renderChart();
  }

  //  Рендер на doughnut диаграма
  function renderChart() {
    const ctx = document.getElementById('gradesChart').getContext('2d');
    const counts = {};
    archive.forEach(a => counts[a.grade] = (counts[a.grade] || 0) + 1);
    const labels = Object.keys(counts);
    const data = Object.values(counts);

    if (chartInst) chartInst.destroy();
    chartInst = new Chart(ctx, {
      type: 'doughnut',
      data: {
        labels,
        datasets: [{
          data,
          backgroundColor: ['#42A5F5', '#EC407A', '#FFA726'],
          hoverOffset: 20
        }]
      },
      options: {
        cutout: '50%',
        plugins: {
          legend: { position: 'bottom' }
        }
      }
    });
  }

  //  „Добави тест“
  addUpBtn.addEventListener('click', () => {
    const subject = prompt('Предмет:', 'Ново...');
    const type = prompt('Тип:', 'Тест');
    const date = prompt('Дата (ГГГГ-MM-DD):', '');
    const time = prompt('Час (HH:MM):', '');
    const loc = prompt('Локация:', '');
    if (subject && date) {
      upcoming.push({ subject, type, date, time, loc, status: 'Предстои' });
      renderTables();
    }
  });

  //  Филтри и търсене
  subjFilter.addEventListener('change', renderTables);
  searchInp.addEventListener('input', renderTables);

  //  Плавен scroll-to-top
  window.addEventListener('scroll', () => {
    scrollBtn.classList.toggle('show', window.scrollY > 300);
  });
  scrollBtn.addEventListener('click', () => {
    window.scrollTo({ top: 0, behavior: 'smooth' });
  });

  // начален рендер
  renderTables();
});
