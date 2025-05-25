document.addEventListener('DOMContentLoaded', () => {
  //  Toggle sidebar
  const sideMenu = document.getElementById('sideMenu');
  document.querySelectorAll('.menu-btn').forEach(btn => {
    btn.addEventListener('click', () => {
      sideMenu.classList.toggle('open');
    });
  });

  //  Accordion FAQ
  document.querySelectorAll('.acc-btn').forEach(btn => {

    const content = btn.nextElementSibling;
    btn.addEventListener('click', () => {
      const isActive = btn.classList.toggle('active');
      if (isActive) {
        content.style.maxHeight = content.scrollHeight + 'px';
      } else {
        content.style.maxHeight = '0';
      }
    });
  });

  //  Search filter for FAQ
  const helpSearch = document.getElementById('helpSearch');
  if (helpSearch) {
    helpSearch.addEventListener('input', () => {
      const term = helpSearch.value.toLowerCase();
      document.querySelectorAll('.acc-item').forEach(item => {
        const question = item.querySelector('.acc-btn').textContent.toLowerCase();
        item.style.display = question.includes(term) ? '' : 'none';
      });
    });
  }

  //  Contact form submit
  const helpForm = document.getElementById('helpForm');
  if (helpForm) {
    helpForm.addEventListener('submit', e => {
      e.preventDefault();
      alert('Вашата заявка е изпратена. Ще се свържем с вас скоро.');
      helpForm.reset();
    });
  }

  //  Scroll-to-top button
  const scrollBtn = document.getElementById('scrollTopBtn');
  if (scrollBtn) {
    window.addEventListener('scroll', () => {
      scrollBtn.classList.toggle('show', window.scrollY > 300);
    });
    scrollBtn.addEventListener('click', () => {
      window.scrollTo({ top: 0, behavior: 'smooth' });
    });
  }
});
