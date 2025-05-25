// js/select-school.js
document.getElementById('schoolForm').addEventListener('submit', function(e) {
  e.preventDefault();
  const sel = document.getElementById('schoolSelect').value;
  if (!sel) return;
  // Запазваме избраното училище (примерно в localStorage)
  localStorage.setItem('selectedSchool', sel);
  // Прехвърляме към dashboard
  window.location.href = 'dashboard.html';
});
