Feature: Settings
    In order to configure the Channel 9 MSDN plugin
    As an administrator
    I want to access settings

Background:
    Given a settings object


Scenario: Properties
    Then the settings should have a description of 'Channel 9 (MSDN)'
    And the settings should have an image
    And the settings should have an ID

Scenario: Link
    Then the settings should have the link 'http://github.com/ArildF/Channel-9-Playon-Plugin' 

Scenario: Name
    Then the settings should have the name 'Channel 9 (MSDN)'

Scenario: Unsupported features
    Then checking for updates should not be supported
    And configuring options should not be supported

Scenario: Should not require login
    Then login should not be required

