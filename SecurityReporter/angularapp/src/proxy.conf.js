const PROXY_CONFIG = [
  {
    context: [
      "/Project",
      "/role",
      "/login",
      "/logout",
      "/api/project-reports",
      "/api/dashboard",
        "/profile",
        "/email", 
    ],
    target: "https://localhost:7075",
    secure: false
  }
]

module.exports = PROXY_CONFIG;
