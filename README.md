﻿# PTMK<br><br>

Help:<br>
ptmk 1 - Create \"Person\" table;<br>
ptmk 2 [string] [string] [string] [yyyy/MM/dd] [Female/Male] - Create record with given Surname, Name, Patronymic, DoB, and Sex.<br>
[Local Date Format Required];<br>
ptmk 3 - Print Persons data by distinct Name and DoB;<br>
ptmk 4 - Fill 1,000,000 records AND 100 records of the Males with the first \"F\" character in Surname;<br>
ptmk 5 - Select Males with the first \"F\" in Surname;<br>
ptmk 6A - Execute optimized query by LINQ-Query-Method;<br>
ptmk 6B - Execute optimized query by SqlDataReader.<br><br>


Tests.<br>
Anomalies marked by Exclamation sign.<br><br>

1124 records.<br>
Before optimization.<br>
By EF GetAll Method.<br>
Cold requests:<br><br>

537.5176 ms<br>
597.0488 ms<br>
695.2239 ms<br>
510.9803 ms<br>
564.1771 ms<br>
475.2996 ms<br>
541.4905 ms<br>
525.8494 ms<br>
474.1392 ms<br>
535.5511 ms<br>
AVG 545.72775 ms<br><br>

Hot requests:<br><br>

161.2619 ms<br>
139.7067 ms<br>
371.3239 ms<br>
190.3328 ms<br>
137.2133 ms<br>
133.0465 ms<br>
172.7666 ms<br>
262.9783 ms<br>
132.3585 ms<br>
147.9401 ms<br>
AVG 184.89286 ms<br><br>

After optimization:<br>
Cold requests.<br>
LINQ Query by EF Method:<br><br>

712.0155 ms !<br>
329.4074 ms<br>
254.1894 ms<br>
2846.3501 ms !<br>
291.8332 ms<br>
2913.9548 ms !<br>
289.0225 ms<br>
262.86 ms<br>
266.0127 ms<br>
311.1103 ms<br>
338.4281 ms<br>
244.4955 ms<br>
232.4922 ms<br>
324.5281 ms<br>
275.5185 ms<br>
246.1703 ms<br>
2711.9551 ms !<br>
291.1639 ms<br>
298.7958 ms<br>
281.3689 ms<br>
AVG 283.5873 ms<br><br>

LINQ Query Explicit:<br>
992.015 ms<br>
989.0079 ms<br>
973.1295 ms<br>
985.718 ms<br>
956.9562 ms<br>
924.3208 ms<br>
973.255 ms<br>
984.5813 ms<br>
924.9144 ms<br>
925.3822 ms<br>
AVG 962.92803 ms<br><br>

SqlDataReader:<br>
416.7033 ms<br>
331.7699 ms<br>
345.4472 ms<br>
333.0646 ms<br>
307.133 ms<br>
301.729 ms<br>
255.3908 ms<br>
288.6991 ms<br>
334.6306 ms<br>
268.4055 ms<br>
612.0806 ms !<br>
352.3068 ms<br>
302.8113 ms<br>
295.5639 ms<br>
275.9626 ms<br>
AVG 314.9727
