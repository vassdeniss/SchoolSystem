document.addEventListener('DOMContentLoaded', () => {
  //  Toggle sidebar
  const menuBtn = document.getElementById('menu-btn');
  const sideMenu = document.getElementById('sideMenu');
  menuBtn.addEventListener('click', () => {
    sideMenu.classList.toggle('open');
  });

  //  Демонстрационни данни
  const upcoming = [
    { subject: 'Математика', type: 'Тест', date: '2025-05-10', time: '10:00', loc: 'Ауд. 5', status: 'Предстои' },
    { subject: 'Български', type: 'Контролно', date: '2025-05-12', time: '12:00', loc: 'Онлайн', status: 'Предстои' }
  ];
  const archive = [
    { subject: 'Физика', type: 'Тест', date: '2025-04-20', grade: '5.00' },
    { subject: 'История', type: 'Писмено', date: '2025-04-22', grade: '6.00' },
    { subject: 'Английски', type: 'Писмено', date: '2025-04-22', grade: '4.00' }
  ];

  //  Попълваме таблицата с предстоящи
  const tbl = document.getElementById('testsTable').querySelector('tbody');
  upcoming.forEach(t => {
    const tr = document.createElement('tr');
    tr.innerHTML = `
        <td>${t.subject}</td>
        <td>${t.type}</td>
        <td>${t.date}</td>
        <td>${t.time}</td>
        <td>${t.loc}</td>
        <td>${t.status}</td>
      `;
    tbl.appendChild(tr);
  });

  //  Попълваме архивната таблица
  const atbl = document.getElementById('archiveTable').querySelector('tbody');
  archive.forEach(a => {
    const tr = document.createElement('tr');
    tr.innerHTML = `
        <td>${a.subject}</td>
        <td>${a.type}</td>
        <td>${a.date}</td>
        <td>${a.grade}</td>
      `;
    atbl.appendChild(tr);
  });

  //  Филтър & търсене
  document.getElementById('testSearch').addEventListener('input', e => {
    const term = e.target.value.toLowerCase();
    document.querySelectorAll('#testsTable tbody tr').forEach(row => {
      row.style.display = row.textContent.toLowerCase().includes(term) ? '' : 'none';
    });
  });

  //  Рисуваме диаграмата от архивните оценки
  const labels = archive.map(a => a.subject);
  const data = archive.map(a => parseFloat(a.grade));
  const ctx = document.getElementById('gradesChart').getContext('2d');
  new Chart(ctx, {
    type: 'bar',
    data: {
      labels: labels,
      datasets: [{
        label: 'Оценка',
        data: data,
        backgroundColor: 'rgba(0,184,184,0.6)',
        borderColor: 'rgba(0,184,184,1)',
        borderWidth: 1
      }]
    },
    options: {
      scales: {
        y: { beginAtZero: true, max: 6 }
      },
      plugins: { legend: { display: false } }
    }
  });

  //  Scroll-to-top бутон
  const scrollBtn = document.getElementById('scrollTopBtn');
  window.addEventListener('scroll', () =>
    scrollBtn.classList.toggle('show', window.scrollY > 300)
  );
  scrollBtn.addEventListener('click', () =>
    window.scrollTo({ top: 0, behavior: 'smooth' })
  );
});
