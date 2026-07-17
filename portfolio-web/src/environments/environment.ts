export const environment = {
  production: true,
  /** Base URL of the Portfolio API. */
  apiUrl: 'http://localhost:5014',
  /** Which public portfolio profile to render. */
  portfolioProfileId: 1,
  /**
   * The public site reads data through the API, which requires a JWT with the
   * Public (or Admin) role. These are the seeded read-only credentials created
   * by 12_Portfolio_Auth_Schema.sql. Change them if you rotate the seed user.
   */
  publicCredentials: {
    userName: 'public',
    password: 'Public@123'
  }
};
