document.addEventListener('DOMContentLoaded', () => {
  const burgerBtn = document.getElementById('burgerBtn');
  const sideMenu = document.getElementById('sideMenu');
  const scrollBtn = document.getElementById('scrollTopBtn');
  const editBtn = document.getElementById('editBtn');
  const form = document.getElementById('schoolForm');
  let isEditing = false;

  // Sidebar toggle
  burgerBtn.addEventListener('click', () => {
    sideMenu.classList.toggle('open');
  });

  // Scroll-to-top
  window.addEventListener('scroll', () => {
    scrollBtn.classList.toggle('show', window.scrollY > 300);
  });
  scrollBtn.addEventListener('click', () => {
    window.scrollTo({ top: 0, behavior: 'smooth' });
  });

  // Edit-mode toggle
  editBtn.addEventListener('click', () => {
    isEditing = !isEditing;
    if (isEditing) {
      document.body.classList.add('editing');
      Array.from(form.elements).forEach(el => el.disabled = false);
      editBtn.textContent = 'Запази';
    } else {
      document.body.classList.remove('editing');
      Array.from(form.elements).forEach(el => el.disabled = true);
      editBtn.textContent = 'Редактирай';

    }
  });
});
