Feature: Channel 9 plugin
	In order to watch Channel 9 videos on my XBox
	As an XBox owner using the PlayOn media server
	I want to have a plugin for PlayOn that streams videos from Channel 9 

Background:
	Given an RSS file 'Channel9.rss'
	And a Channel 9 provider
    And a settings object

Scenario: Description
    Then the settings should have a description of 'Channel 9 (MSDN)'
    And the settings should have an image
    

Scenario: Have an RSS root folder
	When I retrieve the children of the root
	Then there should be only 1 child
	And child 0 should be named 'RSS'

Scenario: Retrieve RSS items
    When I retrieve the children of 'root=>RSS'
    Then child 1 should have these attributes:
    |Name       |Value                                      |
    |Title       |Visual Studio LightSwitch - Beyond the Basics|
