# Day 01 Sonar Sweep

## Problem
As the submarine drops below the surface of the ocean, it automatically performs a sonar sweep of the nearby sea floor. On a small screen, the sonar sweep report (your puzzle input) appears: each line is a measurement of the sea floor depth as the sweep looks further and further away from the submarine.

### Part One
For example, suppose you had the following report:

```
199
200
208
210
200
207
240
269
260
263
```

This report indicates that, scanning outward from the submarine, the sonar sweep found depths of 199, 200, 208, 210, and so on.

The first order of business is to figure out how quickly the depth increases, just so you know what you're dealing with - you never know if the keys will get carried into deeper water by an ocean current or a fish or something.

To do this, count the number of times a depth measurement increases from the previous measurement. (There is no measurement before the first measurement.) In the example above, the changes are as follows:
```
199 (N/A - no previous measurement)
200 (increased)
208 (increased)
210 (increased)
200 (decreased)
207 (increased)
240 (increased)
269 (increased)
260 (decreased)
263 (increased)
```
In this example, there are `7` measurements that are larger than the previous measurement.

**How many measurements are larger than the previous measurement?**

### Part Two
Considering every single measurement isn't as useful as you expected: there's just too much noise in the data.

Instead, consider sums of a three-measurement sliding window. Again considering the above example:
```
199  A      
200  A B    
208  A B C  
210    B C D
200  E   C D
207  E F   D
240  E F G  
269    F G H
260      G H
263        H
```
Start by comparing the first and second three-measurement windows. The measurements in the first window are marked `A` (`199`, `200`, `208`); their sum is `199 + 200 + 208 = 607`. The second window is marked `B` (`200`, `208`, `210`); its sum is `618`. The sum of measurements in the second window is larger than the sum of the first, so this first comparison increased.

Your goal now is to count **the number of times the sum of measurements in this sliding window increases** from the previous sum. So, compare A with B, then compare B with C, then C with D, and so on. Stop when there aren't enough measurements left to create a new three-measurement sum.

In the above example, the sum of each three-measurement window is as follows:
```
A: 607 (N/A - no previous sum)
B: 618 (increased)
C: 618 (no change)
D: 617 (decreased)
E: 647 (increased)
F: 716 (increased)
G: 769 (increased)
H: 792 (increased)
```
In this example, there are `5` sums that are larger than the previous sum.

Consider sums of a three-measurement sliding window. **How many sums are larger than the previous sum?**

## Solutions
The first thing I did was store the input data in a file and read it from the program. The data is then stored as an integer array.

For Part One, we just need to compare each pair of successive elements, filter the ones where the second is larger than the first, and count how many we got.

```cs
array
.Where((current, index) => index >= 1 && current > _depthArray[index - 1])
.Count();
```

For Part Two, the sum of the ith window is:

```sum[i] = arr[i] + arr[i + 1] + arr[i + 2]```

While the sum of the next window is:

```sum[i + 1] = arr[i + 1] + arr[i + 2] + arr[i + 3]```

The case we need to satisfy is:

```sum[i + 1] > sum[i]```

We can simplify the statements above to:
```arr[i + 3] > arr[i]```

Using these calculations, we can then utilize the solution from Part One by swapping the constant with a variable `windowSize` which we could assign 1 or 3 to.
```cs
array
.Where((current, index) => index >= windowSize && current > _depthArray[index - windowSize])
.Count();
```

## Notes
LINQ is very handy in this scenario!
- [Enumerable.Where](https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.where?view=net-6.0)
- [Enumerable.Count](https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.count?view=net-6.0)

I did not include my actual data in the project. Please see this [Reddit comment](https://www.reddit.com/r/adventofcode/comments/e7khy8/comment/fa13hb9/?utm_source=share&utm_medium=web2x&context=3) for more details.
> Please don't share your input anywhere as that makes it easier for unscrupulous folks to reverse-engineer all the hard work that [/u/topaz2078](https://www.reddit.com/user/topaz2078) has put into this event.