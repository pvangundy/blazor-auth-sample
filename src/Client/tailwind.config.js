module.exports = {
  content: [
    "./**/*.{razor,html}",
    "./**/*.cshtml"
  ],
  theme: {
    extend: {
      colors: {
        "ios-background": "#f2f2f7",
        "ios-card": "rgba(255,255,255,0.85)",
        "ios-primary": "#0a84ff"
      },
      fontFamily: {
        sans: ["-apple-system", "BlinkMacSystemFont", "'SF Pro Display'", "'SF Pro Text'", "'Helvetica Neue'", "Helvetica", "Arial", "sans-serif"]
      },
      boxShadow: {
        ios: "0 10px 30px rgba(0, 0, 0, 0.1)"
      },
      borderRadius: {
        ios: "28px"
      }
    }
  },
  plugins: []
};
