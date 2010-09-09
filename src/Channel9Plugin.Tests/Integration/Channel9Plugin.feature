Feature: Channel 9 plugin
	In order to watch Channel 9 videos on my XBox
	As an XBox owner using the PlayOn media server
	I want to have a plugin for PlayOn that streams videos from Channel 9 

Background:
	Given a file 'Channel9.rss' at the URL 'http://channel9.msdn.com/Feeds/RSS/'
    And a file 'Shows_Channel9.htm' at the URL 'http://channel9.msdn.com/shows/'
    And a file 'ButWhy.rss' at the URL 'http://channel9.msdn.com/shows/ButWhy/feed/wmvhigh'
	And a Channel 9 provider
    And a settings object

Scenario: Description
    Then the settings should have a description of 'Channel 9 (MSDN)'
    And the settings should have an image
    

Scenario: Have an RSS root folder
	When I retrieve the children of the root
	Then there should be 2 children
	And child 0 should be named 'RSS'
    And child 1 should be named 'Shows'

Scenario: Retrieve RSS items
    When I retrieve the payload of 'root=>RSS'
    Then child 1 should have these attributes:
    |Name       |Value                                      |
    |Title       |Visual Studio LightSwitch - Beyond the Basics|

Scenario: RSS item count
    When I retrieve the payload of 'root=>RSS'
    Then there should be 25 children

Scenario: Folders item count
    When I retrieve the payload of 'root=>Shows'
    Then there should be 21 children

Scenario: Shows folder title
    When I retrieve the payload of 'root=>Shows'
    And I examine child #1 as a folder
    Then the folder should have a title of 'In the Office'

Scenario: Media URL
    When I retrieve the payload of 'root=>RSS'
    And I examine child #1 as a video file
    Then the video file should have a media URL of 'http://ecn.channel9.msdn.com/o9/ch9/6296/566296/LightSwitchBeyondBasics_ch9.wmv'

Scenario: Shows media URL
    When I retrieve the payload of 'root=>Shows=>But Why?'
    And I examine child #1 as a video file
    Then the video file should have a media URL of 'http://ecn.channel9.msdn.com/o9/ch9/2730/562730/Giblets3_ch9.wmv'

Scenario: Duration
    When I retrieve the payload of 'root=>RSS'
    And I examine child #1 as a video file
    Then the video file should have a duration of 2604000

Scenario: Thumbnail
    When I retrieve the payload of 'root=>RSS'
    And I examine child #1 as a video file
    Then the video file should have a thumbnail 'http://ecn.channel9.msdn.com/o9/ch9/6296/566296/LightSwitchBeyondBasics_320_ch9.png'

Scenario: Publication date
    When I retrieve the payload of 'root=>RSS'
    And I examine child #1 as a video file
    Then the video file should have a publication date of 'Wed, 11 Aug 2010 19:04:00 GMT'

Scenario: Don't retrieve children of media
    When I retrieve the payload of 'root=>RSS=>1' without children
    Then the payload should be a media file

Scenario: Sort order
    When I retrieve the payload of 'root=>RSS'
    Then the children should have sort prefixes ordered by publication date descending

Scenario: Media XML
    When I retrieve media child #1 of 'root=>RSS'
    And I resolve the item into XML
    Then the xml should contain "/media/url[@type='wmv']"
    And the xml should contain "/media/url[.='http://ecn.channel9.msdn.com/o9/ch9/6296/566296/LightSwitchBeyondBasics_ch9.wmv']"
