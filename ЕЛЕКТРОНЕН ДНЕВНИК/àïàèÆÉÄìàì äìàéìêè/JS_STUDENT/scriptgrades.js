document.addEventListener('DOMContentLoaded', () => {
  const menuBtn = document.getElementById('menu-btn');
  const sideMenu = document.getElementById('sideMenu');


  console.log('menuBtn=', menuBtn, 'sideMenu=', sideMenu);

  menuBtn.addEventListener('click', () => {
    sideMenu.classList.toggle('open');
  });





  //  Превключване на срокове
  const select = document.getElementById('term-select');
  const term1Tables = document.querySelectorAll('.term1');
  const term2Tables = document.querySelectorAll('.term2');

  function switchTerm() {
    const value = select.value;
    term1Tables.forEach(t => t.style.display = value === 'term1' ? 'table' : 'none');
    term2Tables.forEach(t => t.style.display = value === 'term2' ? 'table' : 'none');
  }

  select.addEventListener('change', switchTerm);
  switchTerm();

  //  Scroll‐to‐top бутон
  const scrollBtn = document.getElementById('scrollTopBtn');
  window.addEventListener('scroll', () => {
    scrollBtn.classList.toggle('show', window.scrollY > 300);
  });
  scrollBtn.addEventListener('click', () => {
    window.scrollTo({ top: 0, behavior: 'smooth' });
  });
});
