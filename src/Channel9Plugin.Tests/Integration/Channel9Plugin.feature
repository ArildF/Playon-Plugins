Feature: Channel 9 plugin
	In order to watch Channel 9 videos on my XBox
	As an XBox owner using the PlayOn media server
	I want to have a plugin for PlayOn that streams videos from Channel 9 

Background:
	Given an RSS file 'Channel9.rss'
	And a Channel 9 provider

Scenario: Have an RSS root folder
	When I retrieve the children of the root
	Then there should be only 1 child
	And it should be named 'RSS'