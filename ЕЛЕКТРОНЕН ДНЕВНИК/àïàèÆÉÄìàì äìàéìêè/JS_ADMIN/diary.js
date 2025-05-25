document.addEventListener('DOMContentLoaded', () => {
  const menuBtn     = document.getElementById('menuBtn');
  const sideMenu    = document.getElementById('sideMenu');
  const editPageBtn = document.getElementById('editPageBtn');
  const mainSection = document.querySelector('main.director-page');

  let pageEditing = false;

  // 1) Sidebar toggle
  menuBtn.addEventListener('click', () => {
    sideMenu.classList.toggle('open');
  });

  // 2) Toggle whole-page edit mode
  editPageBtn.addEventListener('click', () => {
    pageEditing = !pageEditing;
    if (pageEditing) {
      mainSection.setAttribute('contenteditable', 'true');
      editPageBtn.textContent = 'Запази';
      mainSection.classList.add('page-editing');
    } else {
      mainSection.removeAttribute('contenteditable');
      editPageBtn.textContent = 'Редактирай';
      mainSection.classList.remove('page-editing');
      // При запазване синхронизираме таблицата
      const activeClass = document.querySelector('.cls-btn.active').dataset.class;
      document.querySelectorAll('#diaryTable tbody tr').forEach((tr, i) => {
        const row = data[activeClass][i];
        row.math = tr.children[2].textContent.trim();
        row.bg   = tr.children[3].textContent.trim();
        row.phy  = tr.children[4].textContent.trim();
      });
      render(activeClass);
    }
  });

  // 3) Примерни данни
  const data = {
    '8': [
      { name:'Иван Иванов', cls:'8', math:5, bg:6, phy:4 },
      { name:'Петър Петров', cls:'8', math:6, bg:5, phy:5 }
    ],
    '9': [
      { name:'Анна Георгиева', cls:'9', math:4, bg:6, phy:5 }
    ],
    '10': [], '11': [], '12': []
  };

  // 4) Рендер функция
  const classBtns = document.querySelectorAll('.cls-btn');
  const tbody     = document.querySelector('#diaryTable tbody');
  function render(classNum) {
    tbody.innerHTML = '';
    (data[classNum] || []).forEach((stu, i) => {
      const tr = document.createElement('tr');
      tr.innerHTML = `
        <td>${stu.name}</td>
        <td>${stu.cls} клас</td>
        <td>${stu.math}</td>
        <td>${stu.bg}</td>
        <td>${stu.phy}</td>
        <td>
          <button class="action-btn edit"   title="Редактирай ред">✏️</button>
          <button class="action-btn delete" title="Изтрий ред">🗑️</button>
        </td>`;
      tbody.appendChild(tr);

      tr.querySelector('.delete').addEventListener('click', () => {
        data[classNum].splice(i,1);
        render(classNum);
      });
      tr.querySelector('.edit').addEventListener('click', () => {
        // при редакция на един ред, включваме му contenteditable
        ['2','3','4'].forEach(idx=>{
          const td = tr.children[idx];
          td.contentEditable = 'true';
          td.focus();
        });
      });
    });
  }

  // 5) Селекция на клас
  classBtns.forEach(btn => {
    btn.addEventListener('click', () => {
      classBtns.forEach(b => b.classList.remove('active'));
      btn.classList.add('active');
      render(btn.dataset.class);
    });
  });
  classBtns[0].classList.add('active');
  render('8');

  // 6) Scroll-to-top
  const scrollBtn = document.getElementById('scrollTopBtn');
  window.addEventListener('scroll', () => {
    scrollBtn.classList.toggle('show', window.scrollY > 300);
  });
  scrollBtn.addEventListener('click', () => {
    window.scrollTo({ top: 0, behavior: 'smooth' });
  });
});
