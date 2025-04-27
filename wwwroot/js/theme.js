function toggleTheme() {
    const body = document.body;
    const themeButton = document.getElementById('themeToggle');

    const isDark = body.classList.contains('dark-theme');

    body.classList.toggle('dark-theme', !isDark);
    body.classList.toggle('light-theme', isDark);

    if (isDark) {
        themeButton.innerHTML = '🌞';
        themeButton.style.backgroundColor = '#fff';
        themeButton.style.color = '#000';
        localStorage.setItem('theme', 'light');
    } else {
        themeButton.innerHTML = '🌙';
        themeButton.style.backgroundColor = '#333';
        themeButton.style.color = '#fff';
        localStorage.setItem('theme', 'dark');
    }
}

document.addEventListener('DOMContentLoaded', function () {
    const body = document.body;
    const themeButton = document.getElementById('themeToggle');

    const savedTheme = localStorage.getItem('theme');

    if (savedTheme === 'dark') {
        body.classList.add('dark-theme');
        themeButton.innerHTML = '🌙';
        themeButton.style.backgroundColor = '#333';
        themeButton.style.color = '#fff';
    } else {
        body.classList.add('light-theme');
        themeButton.innerHTML = '🌞';
        themeButton.style.backgroundColor = '#fff';
        themeButton.style.color = '#000';
    }
});