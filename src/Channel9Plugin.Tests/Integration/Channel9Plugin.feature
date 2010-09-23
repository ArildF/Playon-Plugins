Feature: Channel 9 plugin
    In order to watch Channel 9 videos on my XBox
    As an XBox owner using the PlayOn media server
    I want to have a plugin for PlayOn that streams videos from Channel 9 

Background:
    Given a file 'Channel9.rss' at the URL 'http://channel9.msdn.com/Feeds/RSS/'
    And a file 'Shows_Channel9.htm' at the URL 'http://channel9.msdn.com/shows/'
    And a file 'ButWhy.rss' at the URL 'http://channel9.msdn.com/shows/ButWhy/feed/wmvhigh'
    And a file 'Ping.rss' at the URL 'http://channel9.msdn.com/shows/PingShow/feed/wmvhigh'
    And a Channel 9 provider

    

Scenario: Name and image
    Then the provider should have an image
    And the provider should have the name 'Channel 9 (MSDN)'

Scenario: Invalid feed
    Given a file 'Invalid.rss' at the URL 'http://channel9.msdn.com/Feeds/RSS/'
    Then I should get an error when I browse 'root=>RSS'

Scenario: Have an RSS root folder
    When I browse the root
    Then there should be 2 items
    And item 0 should be named 'RSS'
    And item 1 should be named 'Shows'

Scenario: Retrieve RSS items
    When I browse 'root=>RSS'
    Then item 1 should have these attributes:
    |Name       |Value                                      |
    |Title       |Visual Studio LightSwitch - Beyond the Basics|

Scenario: RSS item count
    When I browse 'root=>RSS'
    Then there should be 25 items

Scenario: Restrict number of items returned
    When I browse the first 5 items of 'root=>RSS'
    Then there should be 5 items

Scenario: Start at non-zero index
    When I browse 4 items starting from index 6 of 'root=>RSS'
    Then there should be 4 items
    And item 1 should have these attributes:
    |Name       |Value          |
    |Title      |Ping 69: Windows Phone 7 adds Voice, Mobile App Match, Bing Taxi, Halo 2600|

Scenario: Retrieve RSS items twice
    When I browse 'root=>RSS'
    And I browse 'root=>RSS'
    Then there should be 25 items

Scenario: Folders item count
    When I browse 'root=>Shows'
    Then there should be 21 items

Scenario: Retrieve show folders twice
    When I browse 'root=>Shows'
    And I browse 'root=>Shows'
    Then there should be 21 items

Scenario: Shows folder title
    When I browse 'root=>Shows'
    And item #1 is a folder
    Then the folder should have a title of 'In the Office'

Scenario: Media URL
    When I browse 'root=>RSS'
    And item #1 is a video file
    Then the video should have a media URL of 'http://ecn.channel9.msdn.com/o9/ch9/6296/566296/LightSwitchBeyondBasics_ch9.wmv'

Scenario: Shows media URL
    When I browse 'root=>Shows=>But Why?'
    And item #1 is a video file
    Then the video should have a media URL of 'http://ecn.channel9.msdn.com/o9/ch9/2730/562730/Giblets3_ch9.wmv'

Scenario: Duration
    When I browse 'root=>RSS'
    And item #1 is a video file
    Then the video should have a duration of 2604000

Scenario: Thumbnail
    When I browse 'root=>RSS'
    And item #1 is a video file
    Then the video should have a thumbnail 'http://ecn.channel9.msdn.com/o9/ch9/6296/566296/LightSwitchBeyondBasics_320_ch9.png'

Scenario: Publication date
    When I browse 'root=>RSS'
    And item #1 is a video file
    Then the video should have a publication date of 'Wed, 11 Aug 2010 19:04:00 GMT'

Scenario: Don't retrieve children of media
    When I browse 'root=>RSS=>1' without children
    Then it should be a media file

Scenario: Sort order
    When I browse 'root=>RSS'
    Then the item names should have sort prefixes ordered by publication date descending

Scenario: Media XML
    When I browse item #1 of 'root=>RSS'
    And I examine the item as XML
    Then the xml should contain "/media/url[@type='wmv']"
    And the xml should contain "/media/url[.='http://ecn.channel9.msdn.com/o9/ch9/6296/566296/LightSwitchBeyondBasics_ch9.wmv']"

Scenario: Missing duration
    When I browse 'root=>Shows=>Ping!'
    Then there should be 25 items

Scenario: Plain text descriptions
    When I browse 'root=>RSS'
    And item #1 is a video file
    Then the video description should not contain HTML tags

Scenario: Feed item without video
    Given a file 'NoVideo.rss' at the URL 'http://channel9.msdn.com/Feeds/RSS/'
    When I browse 'root=>RSS'
    Then there should be 0 items
