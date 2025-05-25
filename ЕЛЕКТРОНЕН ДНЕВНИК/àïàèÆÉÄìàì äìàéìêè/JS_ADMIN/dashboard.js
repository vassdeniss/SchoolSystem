document.addEventListener('DOMContentLoaded', () => {
  const burgerBtn = document.getElementById('burgerBtn');
  const sideMenu = document.getElementById('sideMenu');
  const scrollBtn = document.getElementById('scrollTopBtn');
  const editBtn = document.getElementById('editBtn');
  const mainArea = document.querySelector('main.dashboard-page');
  let isEditing = false;

  // Sidebar toggle
  burgerBtn.addEventListener('click', () => {
    sideMenu.classList.toggle('open');
  });

  // Scroll-to-top button
  window.addEventListener('scroll', () => {
    scrollBtn.classList.toggle('show', window.scrollY > 300);
  });
  scrollBtn.addEventListener('click', () => {
    window.scrollTo({ top: 0, behavior: 'smooth' });
  });

  // Edit-mode toggle
  editBtn.addEventListener('click', () => {
    if (!isEditing) {
      // Enter edit mode
      mainArea.setAttribute('contenteditable', 'true');
      mainArea.style.border = '2px dashed #00b8b8';
      editBtn.textContent = 'Запази';
      isEditing = true;
    } else {
      // Exit edit mode
      mainArea.removeAttribute('contenteditable');
      mainArea.style.border = 'none';
      editBtn.textContent = 'Редактирай';
      isEditing = false;
    }
  });

  // KPI demo data
  document.getElementById('kpiSchools').textContent = 12;
  document.getElementById('kpiUsers').textContent = 234;
  document.getElementById('kpiAvg').textContent = '4.85';
  document.getElementById('kpiAtt').textContent = '96.7 %';

  // Recent logs
  const logs = [
    'Админ добави ново училище',
    'Учител Иванов модифицира оценки',
    'Родител Петров регистрира акаунт'
  ];
  const ulLogs = document.getElementById('logsList');
  logs.forEach(text => {
    const li = document.createElement('li');
    li.textContent = text;
    ulLogs.appendChild(li);
  });

  // Trend chart
  const ctx = document.getElementById('trendChart').getContext('2d');
  new Chart(ctx, {
    type: 'line',
    data: {
      labels: ['Яну', 'Фев', 'Март', 'Апр', 'Май', 'Юни'],
      datasets: [{
        label: 'Среден успех',
        data: [4.5, 4.6, 4.7, 4.8, 4.85, 4.9],
        fill: false,
        borderColor: '#00b8b8',
        tension: 0.2
      }]
    },
    options: {
      responsive: true,
      scales: {
        y: { beginAtZero: false }
      }
    }
  });

  // Top-5 classes
  const top5 = [
    { cls: '12A', avg: 5.40 },
    { cls: '11B', avg: 5.20 },
    { cls: '10C', avg: 5.15 },
    { cls: '9A', avg: 5.10 },
    { cls: '8D', avg: 5.00 }
  ];
  const container = document.querySelector('.top5-cards');
  top5.forEach(o => {
    const div = document.createElement('div');
    div.className = 'top5-card';
    div.innerHTML = `<h4>${o.cls}</h4><p>${o.avg.toFixed(2)}</p>`;
    container.appendChild(div);
  });

  // Upcoming events
  let events = [
    'Родителска среща — 25.05.2025',
    'Контролно по математика — 28.05.2025'
  ];
  const evList = document.getElementById('eventsList');
  const evBtn = document.getElementById('addEventBtn');
  function renderEvents() {
    evList.innerHTML = '';
    events.forEach(e => {
      const li = document.createElement('li');
      li.textContent = e;
      evList.appendChild(li);
    });
  }
  evBtn.addEventListener('click', () => {
    const txt = prompt('Опиши събитието (например „Име — Дата“):');
    if (txt) {
      events.push(txt);
      renderEvents();
    }
  });
  renderEvents();
});
