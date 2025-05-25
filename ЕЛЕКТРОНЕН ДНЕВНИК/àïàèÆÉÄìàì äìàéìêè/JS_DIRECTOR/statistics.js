document.addEventListener('DOMContentLoaded', () => {
  // sidebar toggle
  document.getElementById('burgerBtn')
    .addEventListener('click', () =>
      document.getElementById('sideMenu').classList.toggle('open')
    );

  // данни за диаграмите
  const months = ['Ян', 'Фев', 'Мар', 'Апр', 'Май', 'Юни'];
  const avgGrades = [4.7, 4.8, 4.9, 4.85, 4.9, 4.88];
  const failCounts = [2, 3, 1, 4, 2, 2];
  const attendanceData = [98.2, 1.8];

  // Top 5 класове
  const top = [
    { cls: '10А', avg: 5.00 },
    { cls: '11Б', avg: 4.90 },
    { cls: '9В', avg: 4.85 },
    { cls: '12Г', avg: 4.80 },
    { cls: '8А', avg: 4.75 }
  ];

  // линейна
  new Chart(
    document.getElementById('lineChart').getContext('2d'),
    {
      type: 'line',
      data: {
        labels: months,
        datasets: [{
          label: 'Среден успех',
          data: avgGrades,
          borderColor: '#00b8b8',
          fill: false,
          tension: 0.3
        }]
      },
      options: { scales: { y: { beginAtZero: true, max: 6 } } }
    }
  );

  // бар (контролни)
  new Chart(
    document.getElementById('barChart').getContext('2d'),
    {
      type: 'bar',
      data: {
        labels: months,
        datasets: [{
          label: 'Неиздържани',
          data: failCounts,
          backgroundColor: '#e91e63'
        }]
      },
      options: { scales: { y: { beginAtZero: true } } }
    }
  );

  // пай (присъствия)
  new Chart(
    document.getElementById('pieChart').getContext('2d'),
    {
      type: 'pie',
      data: {
        labels: ['Присъствия', 'Отсъствия'],
        datasets: [{
          data: attendanceData,
          backgroundColor: ['#00b8b8', '#f44336']
        }]
      }
    }
  );

  // хоризонтален бар за Топ 5
  new Chart(
    document.getElementById('topBarChart').getContext('2d'),
    {
      type: 'bar',
      data: {
        labels: top.map(o => o.cls),
        datasets: [{
          label: 'Среден успех',
          data: top.map(o => o.avg),
          backgroundColor: '#00b8b8'
        }]
      },
      options: {
        indexAxis: 'y',
        scales: {
          x: { beginAtZero: true, max: 6 }
        },
        plugins: { legend: { display: false } }
      }
    }
  );

  // Експорт PDF (print)
  document.getElementById('exportPdf')
    .addEventListener('click', () => window.print());

  // филтри (placeholder)
  document.getElementById('periodSelect')
    .addEventListener('change', e => console.log('Period', e.target.value));
  document.getElementById('levelSelect')
    .addEventListener('change', e => console.log('Level', e.target.value));
});
