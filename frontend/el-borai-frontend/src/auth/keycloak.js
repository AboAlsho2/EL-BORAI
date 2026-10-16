import Keycloak from "keycloak-js";

const keycloak = new Keycloak({
  url: "http://localhost:8180",
  realm: "EL-BORAI",
  clientId: "elborai-frontend",
});

export default keycloak;