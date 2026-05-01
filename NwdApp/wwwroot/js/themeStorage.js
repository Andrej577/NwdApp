window.themeStorage = {
    getDarkMode: () => localStorage.getItem("darkMode") === "true",
    setDarkMode: (value) => localStorage.setItem("darkMode", value ? "true" : "false")
};