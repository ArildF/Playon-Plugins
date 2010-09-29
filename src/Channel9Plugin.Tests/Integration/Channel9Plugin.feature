Feature: Channel 9 plugin
    In order to watch Channel 9 videos on my XBox
    As an XBox owner using the PlayOn media server
    I want to have a plugin for PlayOn that streams videos from Channel 9 

Background:
    Given the following files for URLs:
    |Url                                                    |File                   |
    |http://channel9.msdn.com/Feeds/RSS/                    |Channel9.rss           |
    |http://channel9.msdn.com/Browse/Shows?sort=atoz&page=1 |Shows1.html            |
    |http://channel9.msdn.com/Browse/Shows?sort=atoz&page=2 |Shows2.html            |
    |http://channel9.msdn.com/Browse/Shows?sort=atoz&page=3 |Shows3.html            |
    |http://channel9.msdn.com/Browse/Shows?sort=atoz&page=4 |Shows4.html            |
    |http://channel9.msdn.com/Browse/Shows?sort=atoz&page=5 |Shows5.html            |
    |http://channel9.msdn.com/Browse/Series?sort=atoz&page=1    |Series1.html            |
    |http://channel9.msdn.com/Shows/ButWhy/RSS              |ButWhy.rss             |
    |http://channel9.msdn.com/Browse/Tags?page=1            |Tags1.html             |
    |http://channel9.msdn.com/Browse/Tags?page=2            |Tags2.html             |
    |http://channel9.msdn.com/Shows/PingShow/RSS            |Ping.rss               |
    |http://channel9.msdn.com/Tags/ajax/RSS                 |Ajax.rss               |
    |http://channel9.msdn.com/Series/History/RSS            |Series.rss             |
    And a Channel 9 provider

    

Scenario: Name and image
    Then the provider should have an image
    And the provider should have the name 'Channel 9 (MSDN)'

Scenario: Invalid feed
    Given a file 'Invalid.rss' at the URL 'http://channel9.msdn.com/Feeds/RSS/'
    Then I should get an error when I browse 'root=>RSS'

Scenario: Have an RSS root folder
    When I browse the root
    Then there should be 4 items
    And item 0 should be named 'RSS'
    And item 1 should be named 'Shows'
    And item 2 should be named 'Tags'
    And item 3 should be named 'Series'

Scenario: Retrieve RSS items
    When I browse 'root=>RSS'
    Then item 1 should have these attributes:
    |Name       |Value                                      |
    |Title       |TWC9: VB for Windows Phone 7, ASP.NET Vulnerability, WCF Services, String Formatting Cheat Sheet|

Scenario: RSS item count
    When I browse 'root=>RSS'
    Then there should be 24 items

Scenario: Restrict number of items returned
    When I browse the first 5 items of 'root=>RSS'
    Then there should be 5 items

Scenario: Start at non-zero index
    When I browse 4 items starting from index 6 of 'root=>RSS'
    Then there should be 4 items
    And item 1 should have these attributes:
    |Name       |Value          |
    |Title      |Don McCrady - Parallelism in C++ Using the Concurrency Runtime|

Scenario: Retrieve RSS items twice
    When I browse 'root=>RSS'
    And I browse 'root=>RSS'
    Then there should be 24 items

Scenario: Folders item count
    When I browse 'root=>Shows'
    Then there should be 50 items

Scenario: Retrieve show folders twice
    When I browse 'root=>Shows'
    And I browse 'root=>Shows'
    Then there should be 50 items

Scenario: Shows folder title
    When I browse 'root=>Shows'
    And item #1 is a folder
    Then the folder should have a title of '10-4'

Scenario: Media URL
    When I browse 'root=>RSS'
    And item #1 is a video file
    Then the video should have a media URL of 'http://ecn.channel9.msdn.com/o9/ch9/18d0/bd9d1134-7051-4bc3-a562-9dfa013a18d0/TWC9Sept24_2MB_ch9.wmv'

Scenario: Shows media URL
    When I browse 'root=>Shows=>But Why?'
    And item #1 is a video file
    Then the video should have a media URL of 'http://ecn.channel9.msdn.com/o9/ch9/2730/562730/Giblets3_2MB_ch9.wmv'

Scenario: Duration
    When I browse 'root=>RSS'
    And item #1 is a video file
    Then the video should have a duration of 946000

Scenario: Thumbnail
    When I browse 'root=>RSS'
    And item #1 is a video file
    Then the video should have a thumbnail 'http://ecn.channel9.msdn.com/o9/ch9/18d0/bd9d1134-7051-4bc3-a562-9dfa013a18d0/TWC9Sept24_100_ch9.jpg'

Scenario: Publication date
    When I browse 'root=>RSS'
    And item #1 is a video file
    Then the video should have a publication date of 'Sat, 25 Sep 2010 02:39:41 GMT'

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
    And the xml should contain "/media/url[.='http://ecn.channel9.msdn.com/o9/ch9/18d0/bd9d1134-7051-4bc3-a562-9dfa013a18d0/TWC9Sept24_2MB_ch9.wmv']"

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

Scenario: Browse Tags
    When I browse 'root=>Tags'
    Then there should be 54 items

Scenario: Browse Tags videos
    When I browse 'root=>Tags=>Ajax (42)'
    Then there should be 12 items
    And item 2 should have these attributes:
    |Name       |Value                                      |
    |Title       |ASP.NET AJAX 4.0 by Fritz Onion           |

Scenario: Browse Series
    When I browse 'root=>Series'
    Then there should be 3 items

Scenario: Browse Series videos
    When I browse 'root=>Series=>The History of Microsoft'
    Then there should be 25 items

Scenario: Html entities should be stripped
    When I browse 'root=>Series=>The History of Microsoft'
    And item #1 is a video file
    Then the video description should not contain HTML entities
