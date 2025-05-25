document.addEventListener('DOMContentLoaded', () => {
  // Sidebar toggle
  const menuBtn = document.querySelector('.navbar .menu-btn');
  const sideMenu = document.getElementById('sideMenu');
  if (!menuBtn || !sideMenu) {
    console.error('Не открих menuBtn или sideMenu!', menuBtn, sideMenu);
  } else {
    menuBtn.addEventListener('click', () => {
      sideMenu.classList.toggle('open');
    });
  }

  // Prev/Next WEEK
  const prevWeekBtn = document.getElementById('prev-week');
  const nextWeekBtn = document.getElementById('next-week');
  const weekLabel = document.getElementById('week-label');
  let currentDate = new Date();

  function updateWeekLabel() {
    const startOfYear = new Date(currentDate.getFullYear(), 0, 1);
    const daysPassed = (currentDate - startOfYear) / 86400000;
    const weekNum = Math.ceil((daysPassed + startOfYear.getDay() + 1) / 7);
    weekLabel.textContent = `Седмица ${weekNum}, ${currentDate.getFullYear()}`;
  }
  prevWeekBtn.addEventListener('click', () => {
    currentDate.setDate(currentDate.getDate() - 7);
    updateWeekLabel();
  });
  nextWeekBtn.addEventListener('click', () => {
    currentDate.setDate(currentDate.getDate() + 7);
    updateWeekLabel();
  });
  updateWeekLabel();

  //  View select
  const viewSelect = document.getElementById('view-select');
  const grid = document.querySelector('.program-grid');
  viewSelect.addEventListener('change', () => {
    grid.classList.toggle('week-view', viewSelect.value === 'week');
    grid.classList.toggle('day-view', viewSelect.value === 'day');
  });

  //  Print
  document.getElementById('print-btn').addEventListener('click', () => {
    window.print();
  });

  //  Export iCal
  document.getElementById('export-ical-btn').addEventListener('click', () => {
    const events = [];
    document.querySelectorAll('.program-grid.week-view .lesson').forEach((cell, i) => {
      const timeText = Array.from(document.querySelectorAll('.time-slot'))[i].textContent;
      events.push({ start: timeText, title: cell.textContent.replace(/\n/g, ' ') });
    });

    let ics = 'BEGIN:VCALENDAR\nVERSION:2.0\nPRODID:-//MyApp//Schedule//BG\n';
    events.forEach((ev, i) => {
      const dt = ev.start.replace(':', '') + '00';
      ics += 'BEGIN:VEVENT\n';
      ics += `UID:event-${i}@myapp\n`;
      ics += `DTSTAMP:${dt}Z\nDTSTART:${dt}Z\nDTEND:${(parseInt(dt) + 100).toString().padStart(6, '0')}Z\n`;
      ics += `SUMMARY:${ev.title}\nEND:VEVENT\n`;
    });
    ics += 'END:VCALENDAR';

    const blob = new Blob([ics], { type: 'text/calendar' });
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = 'schedule.ics';
    a.click();
    URL.revokeObjectURL(url);
  });

  //  Scroll-to-top бутон
  const scrollBtn = document.getElementById('scrollTopBtn');
  window.addEventListener('scroll', () => {
    scrollBtn.classList.toggle('show', window.scrollY > 300);
  });
  scrollBtn.addEventListener('click', () => {
    window.scrollTo({ top: 0, behavior: 'smooth' });
  });
});
