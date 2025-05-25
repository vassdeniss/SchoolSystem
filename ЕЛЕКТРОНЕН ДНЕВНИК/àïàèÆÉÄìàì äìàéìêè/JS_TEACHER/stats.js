document.addEventListener('DOMContentLoaded', () => {
  const kAvg = document.getElementById('kpi-average');
  const kAtt = document.getElementById('kpi-attendance');
  const kMissed = document.getElementById('kpi-missed');
  const gradesCtx = document.getElementById('gradesChart').getContext('2d');
  const attCtx = document.getElementById('attChart').getContext('2d');
  const schedTbody = document.querySelector('#scheduleTable tbody');
  const exportBtn = document.getElementById('export-pdf-btn');

  // Демонстрационни данни
  const data = {
    avg: 4.7,
    attendance: 92,
    missed: 3,
    timeline: ['01', '08', '15', '22', '29'],
    grades: [4.2, 4.5, 4.8, 4.6, 4.7],
    attData: [0.9, 0.92, 0.88, 0.95, 0.92],
    schedule: [
      { day: 'Пон', time: '08:00', cls: 'VII A', subj: 'Математика' },
      { day: 'Вто', time: '10:00', cls: 'VII A', subj: 'Физика' },
      { day: 'Сря', time: '12:00', cls: 'VIII B', subj: 'Български' },
      { day: 'Чет', time: '14:00', cls: 'VIII B', subj: 'История' },
      { day: 'Пет', time: '09:00', cls: 'VII A', subj: 'Английски' }
    ]
  };

  // Напълни KPI
  kAvg.textContent = data.avg.toFixed(2);
  kAtt.textContent = data.attendance + '%';
  kMissed.textContent = data.missed;

  // Графика: успех през срока (линейна)
  new Chart(gradesCtx, {
    type: 'line',
    data: {
      labels: data.timeline,
      datasets: [{
        label: 'Среден успех',
        data: data.grades,
        fill: false,
        borderColor: '#00b8b8',
        tension: 0.1
      }]
    },
    options: {
      scales: {
        y: { beginAtZero: true, max: 6 }
      }
    }
  });

  // Графика: присъствие (бар)
  new Chart(attCtx, {
    type: 'bar',
    data: {
      labels: data.timeline,
      datasets: [{
        label: 'Присъствие',
        data: data.attData,
        backgroundColor: '#00b8b8'
      }]
    },
    options: {
      scales: {
        y: {
          beginAtZero: true,
          max: 1,
          ticks: {
            callback: v => (v * 100).toFixed(0) + '%'
          }
        }
      }
    }
  });

  // Таблица: седмична програма
  data.schedule.forEach(item => {
    const tr = document.createElement('tr');
    tr.innerHTML = `
      <td>${item.day}</td>
      <td>${item.time}</td>
      <td>${item.cls}</td>
      <td>${item.subj}</td>
    `;
    schedTbody.appendChild(tr);
  });

  // PDF Export с jsPDF
  exportBtn.addEventListener('click', () => {
    const { jsPDF } = window.jspdf;
    const doc = new jsPDF('p', 'pt', 'a4');
    doc.setFontSize(16);
    doc.text('Отчет: Статистика и KPI', 40, 40);

    doc.setFontSize(12);
    doc.text(`Среден успех: ${data.avg.toFixed(2)}`, 40, 80);
    doc.text(`Присъствие: ${data.attendance}%`, 40, 100);
    doc.text(`Пропуснати контролни: ${data.missed}`, 40, 120);

    // Вмъкваме първата графика
    const gradeImg = document.getElementById('gradesChart').toDataURL('image/jpeg', 1.0);
    doc.addImage(gradeImg, 'JPEG', 40, 140, 520, 200);

    // Вмъкваме втората графика
    const attImg = document.getElementById('attChart').toDataURL('image/jpeg', 1.0);
    doc.addImage(attImg, 'JPEG', 40, 350, 520, 200);

    doc.save('report.pdf');
  });
});
