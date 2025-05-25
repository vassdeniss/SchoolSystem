function toggleForms() {
  const loginForm = document.getElementById('login-form');
  const registerForm = document.getElementById('register-form');
  loginForm.classList.toggle('active');
  registerForm.classList.toggle('active');
}
