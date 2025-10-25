/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./**/*.razor",
    "./**/*.html"
  ],
  theme: {
    extend: {
      colors: {
        muted: {
          DEFAULT: '#94a3b8',
          foreground: '#64748b'
        }
      },
      fontFamily: {
        display: ['\"SF Pro Display\"', 'ui-sans-serif', 'system-ui'],
        sans: ['\"SF Pro Text\"', 'ui-sans-serif', 'system-ui']
      }
    },
  },
  plugins: [],
}
