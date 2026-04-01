Feature: Login

  @smoke @ui
  Scenario: Successful login with valid credentials
    Given the user opens the login page
    When the "standard" user logs in
    Then the inventory page should be displayed

  @ui
  Scenario: Login fails with invalid credentials
    Given the user opens the login page
    When the "invalid" user logs in
    Then the login error message should be displayed

  