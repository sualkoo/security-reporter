const PROXY_CONFIG = [
  {
    context: [
      "/Project","/role","/login"
    ],
    target: "https://localhost:7075",
    secure: false
  }
]

module.exports = PROXY_CONFIG;
