# AsyncReports
<H1>The Poblem</H1>
We were facing hard times in delivering large reports to the customers. All reports, being them small or big (5MB or more), 
were threated in the same way: all synchronous, request-response pattern based. Well, you can imagine the load and timeout problems we faced, specially when you have thousands of hundreds customers and large reports to deliver.

<H1>The Solution</H1>
While the acquisition of a COTS Reporting tool was under way, I designed and developed this system in C#, which allowed us to deliver reports of any size, in any format in an asynchronous way.

![report request](https://cloud.githubusercontent.com/assets/1160316/20939266/0bfbbfe0-bbe6-11e6-942c-ff630cd528b9.png)
