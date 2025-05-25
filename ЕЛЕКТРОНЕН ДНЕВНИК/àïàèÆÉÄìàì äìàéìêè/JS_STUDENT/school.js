document.addEventListener('DOMContentLoaded', () => {
  //  Toggle sidebar
  const menuBtn = document.getElementById('menu-btn');
  const sideMenu = document.getElementById('sideMenu');
  menuBtn.addEventListener('click', () => {
    sideMenu.classList.toggle('open');
  });

  //  Carousel logic
  const inner = document.getElementById('carouselInner');
  let idx = 0;
  const items = inner.children;
  document.getElementById('carouselPrev').onclick = () => {
    idx = (idx - 1 + items.length) % items.length;
    inner.style.transform = `translateX(-${idx * 100}%)`;
  };
  document.getElementById('carouselNext').onclick = () => {
    idx = (idx + 1) % items.length;
    inner.style.transform = `translateX(-${idx * 100}%)`;
  };
  setInterval(() => document.getElementById('carouselNext').click(), 5000);

  //  Scroll to top
  const scrollBtn = document.getElementById('scrollTopBtn');
  window.addEventListener('scroll', () => {
    scrollBtn.classList.toggle('show', window.scrollY > 300);
  });
  scrollBtn.addEventListener('click', () => {
    window.scrollTo({ top: 0, behavior: 'smooth' });
  });
});
