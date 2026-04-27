/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./src/**/*.{html,ts}"],
  darkMode: "class",
  theme: {
    extend: {
      colors: {
        "primary": "#1c74e9",
        "brand": "#00d084",
        "background-light": "#f6f7f8",
        "background-dark": "#111821",
      },
      fontFamily: {
        "sans": ["Open Sans", "ui-sans-serif", "system-ui"],
        "display": ["Open Sans", "sans-serif"]
      },
      borderRadius: { "DEFAULT": "0.25rem", "lg": "0.5rem", "xl": "0.75rem", "full": "9999px" },
    },
  },
  plugins: []
}
