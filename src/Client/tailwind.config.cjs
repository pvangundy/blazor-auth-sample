module.exports = {
  content: [
    "./**/*.razor",
    "./**/*.html"
  ],
  theme: {
    extend: {
      colors: {
        iosSurface: '#f2f2f7',
        iosPrimary: '#0a84ff',
        iosSecondary: '#5e5ce6'
      },
      fontFamily: {
        sans: ['-apple-system', 'BlinkMacSystemFont', '"Segoe UI"', 'Roboto', 'Helvetica', 'Arial', 'sans-serif']
      },
      boxShadow: {
        ios: '0 10px 30px rgba(0, 0, 0, 0.1)'
      }
    }
  },
  plugins: []
};
