function toggleSidebar() {
  const sidebar = document.getElementById("sidebar");
  if (sidebar.style.left === "0px") {
    sidebar.style.left = "-220px";
  } else {
    sidebar.style.left = "0px";
  }
}

function logout() {

  alert("Излязохте от системата.");
  window.location.href = "index.html"; // пренасочване към login
}
function toggleSidebar() {
  const sidebar = document.getElementById("sidebar");
  sidebar.classList.toggle("open");
}
function toggleMenu() {
  const menu = document.getElementById("sideMenu");
  if (menu.style.left === "0px") {
    menu.style.left = "-250px";
  } else {
    menu.style.left = "0px";
  }
}
const daysContainer = document.getElementById("calendar-days");
const currentDayElem = document.getElementById("current-day");
const monthYearElem = document.getElementById("month-year");

let today = new Date();
let selectedDate = new Date(today.getFullYear(), today.getMonth(), 1);

function renderCalendar() {
  const year = selectedDate.getFullYear();
  const month = selectedDate.getMonth();

  const firstDay = new Date(year, month, 1).getDay();
  const lastDate = new Date(year, month + 1, 0).getDate();

  // Clear previous
  daysContainer.innerHTML = "";

  // Set current left panel
  if (
    today.getFullYear() === year &&
    today.getMonth() === month
  ) {
    currentDayElem.textContent = today.getDate();
  } else {
    currentDayElem.textContent = "-";
  }

  const monthNames = [
    "Януари", "Февруари", "Март", "Април", "Май", "Юни",
    "Юли", "Август", "Септември", "Октомври", "Ноември", "Декември"
  ];

  monthYearElem.textContent = `${monthNames[month].toUpperCase()} - ${year}`;

  // Empty cells before first day
  for (let i = 0; i < firstDay; i++) {
    const empty = document.createElement("div");
    daysContainer.appendChild(empty);
  }

  // Fill the days
  for (let d = 1; d <= lastDate; d++) {
    const dayElem = document.createElement("div");
    dayElem.textContent = d;

    if (
      d === today.getDate() &&
      year === today.getFullYear() &&
      month === today.getMonth()
    ) {
      dayElem.classList.add("today");
    }

    daysContainer.appendChild(dayElem);
  }
}

function changeMonth(offset) {
  selectedDate.setMonth(selectedDate.getMonth() + offset);
  renderCalendar();
}

// Today button
document.querySelector(".today-btn").addEventListener("click", () => {
  selectedDate = new Date();
  renderCalendar();
});

renderCalendar();

const checkboxes = document.querySelectorAll('.task-checkbox');
const progressBar = document.querySelector('progress');
const progressText = document.querySelector('.progress-box p');

function updateProgress() {
  const total = checkboxes.length;
  const done = Array.from(checkboxes).filter(cb => cb.checked).length;
  progressBar.max = total;
  progressBar.value = done;
  progressText.textContent = `${done} от ${total} задачи изпълнени`;
}

checkboxes.forEach(cb => cb.addEventListener('change', updateProgress));


updateProgress();

function logout() {

  sessionStorage.clear();
  localStorage.clear();

  // Пренасочване към login формата
  window.location.href = "login.html"; // промени с реалния път
}
//  Scroll‐to‐top бутон
const scrollBtn = document.getElementById('scrollTopBtn');
window.addEventListener('scroll', () => {
  scrollBtn.classList.toggle('show', window.scrollY > 300);
});
scrollBtn.addEventListener('click', () => {
  window.scrollTo({ top: 0, behavior: 'smooth' });
});
