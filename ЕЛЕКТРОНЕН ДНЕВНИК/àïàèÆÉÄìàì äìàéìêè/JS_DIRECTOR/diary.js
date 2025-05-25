document.addEventListener('DOMContentLoaded', () => {
  //  Toggle sidebar
  const menuBtn = document.getElementById('menuBtn');
  const sideMenu = document.getElementById('sideMenu');
  menuBtn.addEventListener('click', () => {
    sideMenu.classList.toggle('open');
  });

  // Данни за учениците по класове (примерни)
  const data = {
    '8': [
      { name: 'Иван Иванов', cls: '8', math: 5, bg: 6, phy: 4 },
      { name: 'Петър Петров', cls: '8', math: 6, bg: 5, phy: 5 },
      { name: 'Ани Алексова', cls: '8', math: 4, bg: 6, phy: 5 },
    ],
    '9': [
      { name: 'Анна Георгиева', cls: '9', math: 4, bg: 6, phy: 5 },
      { name: 'Георги Георгиев', cls: '9', math: 3, bg: 6, phy: 3 }
    ],
    '10': [
      { name: 'Анна Георгиева', cls: '10', math: 4, bg: 6, phy: 5 },
      { name: 'Александър Алексов', cls: '10', math: 2, bg: 3, phy: 4 }
    ],
    '11': [{ name: 'Сани Георгиева', cls: '11', math: 4, bg: 6, phy: 5 },
      { name: 'Петя Петрова', cls: '11', math: 6, bg: 6, phy: 5 }

    ],
    '12': [
      { name: 'Александра Георгиева', cls: '12', math: 4, bg: 6, phy: 5 },
      { name: 'Анна Георгиева', cls: '12', math: 4, bg: 4, phy: 5 }
    ]
  };

  //  Рендер на таблицата
  const classBtns = document.querySelectorAll('.cls-btn');
  const tbody = document.querySelector('#diaryTable tbody');

  function render(classNum) {
    tbody.innerHTML = '';
    (data[classNum] || []).forEach((stu, i) => {
      const tr = document.createElement('tr');
      tr.innerHTML = `
        <td>${stu.name}</td>
        <td>${stu.cls} клас</td>
        <td contenteditable class="editable">${stu.math}</td>
        <td contenteditable class="editable">${stu.bg}</td>
        <td contenteditable class="editable">${stu.phy}</td>
        <td>
          <button class="action-btn edit"   title="Редактирай">✏️</button>
          <button class="action-btn delete" title="Изтрий">🗑️</button>
        </td>`;
      tbody.appendChild(tr);

      // delete
      tr.querySelector('.delete').addEventListener('click', () => {
        data[classNum].splice(i, 1);
        render(classNum);
      });
      // save (edit)
      tr.querySelector('.edit').addEventListener('click', () => {
        stu.math = tr.children[2].textContent;
        stu.bg = tr.children[3].textContent;
        stu.phy = tr.children[4].textContent;
        render(classNum);
      });
    });
  }

  //  Клас-бутони
  classBtns.forEach(btn => {
    btn.addEventListener('click', () => {
      classBtns.forEach(b => b.classList.remove('active'));
      btn.classList.add('active');
      render(btn.dataset.class);
    });
  });
  // стартира празно/с 8-ми по подразбиране
  classBtns[0].classList.add('active');
  render('8');

  //  Scroll-to-top
  const scrollBtn = document.getElementById('scrollTopBtn');
  window.addEventListener('scroll', () => {
    scrollBtn.classList.toggle('show', window.scrollY > 300);
  });
  scrollBtn.addEventListener('click', () => {
    window.scrollTo({ top: 0, behavior: 'smooth' });
  });
});
