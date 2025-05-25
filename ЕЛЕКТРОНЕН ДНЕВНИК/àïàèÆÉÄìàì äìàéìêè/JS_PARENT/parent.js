// parent/js/parent.js
document.addEventListener('DOMContentLoaded', () => {
    // 1) Toggle sidebar
    const menuBtn  = document.getElementById('menu-btn');
    const sideMenu = document.getElementById('sideMenu');
    menuBtn.addEventListener('click', () => {
      sideMenu.classList.toggle('open');
    });
  
    // 2) Scroll-to-top button
    const scrollBtn = document.getElementById('scrollTopBtn');
    window.addEventListener('scroll', () => {
      scrollBtn.classList.toggle('show', window.scrollY > 300);
    });
    scrollBtn.addEventListener('click', () => {
      window.scrollTo({ top: 0, behavior: 'smooth' });
    });
  
    // 3) Данни за демонстрация (с твоя масив data)
    const data = {
      ivan: {
        grades: [
          { subj:'Математика', date:'2025-05-01', grade:'5.50' },
          { subj:'БЕЛ',          date:'2025-04-28', grade:'6.00' }
        ],
        absences: ['2025-04-20', '2025-05-05'],
        messages: [
          { title:'По БЕЛ',     text:'Моля обърнете внимание на домашното.' },
          { title:'По Матем.',  text:'В понеделник има тест.' }
        ]
      }
    };
  
    // 4) Рендер на данните (както ти вече имаш)
    const gradesBody = document.getElementById('grades-body');
    const absCnt     = document.getElementById('absences-count');
    const absList    = document.getElementById('absences-list');
    const msgAcc     = document.getElementById('msg-accordion');
  
    function render() {
      const d = data.ivan;
  
      // Оценки
      gradesBody.innerHTML = '';
      d.grades.forEach(g => {
        const tr = document.createElement('tr');
        tr.innerHTML = `<td>${g.subj}</td><td>${g.date}</td><td>${g.grade}</td>`;
        gradesBody.appendChild(tr);
      });
  
      // Отсъствия
      absCnt.textContent = d.absences.length;
      absList.innerHTML = '';
      d.absences.forEach(day => {
        const li = document.createElement('li');
        li.textContent = day;
        absList.appendChild(li);
      });
  
      // Съобщения
      msgAcc.innerHTML = '';
      d.messages.forEach(m => {
        const item = document.createElement('div');
        item.className = 'acc-item';
        item.innerHTML = `
          <button class="acc-btn">${m.title}</button>
          <div class="acc-content"><p>${m.text}</p></div>
        `;
        msgAcc.appendChild(item);
      });
  
      // Акордеон функционалност
      document.querySelectorAll('.acc-btn').forEach(btn => {
        const cnt = btn.nextElementSibling;
        btn.addEventListener('click', () => {
          const open = btn.classList.toggle('active');
          cnt.style.maxHeight = open ? cnt.scrollHeight + 'px' : '0';
        });
      });
    }
  
    render();
  });
  