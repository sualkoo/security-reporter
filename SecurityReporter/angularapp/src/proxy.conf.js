const PROXY_CONFIG = [
  {
    context: [
      "/Project",
      "/project-reports",
      "/dashboard"
    ],
    target: "https://localhost:7075",
    secure: false
  }
]

module.exports = PROXY_CONFIG;
