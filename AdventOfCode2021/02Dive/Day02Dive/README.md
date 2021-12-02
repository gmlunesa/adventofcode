# Day 02 Dive

## Problem
Now, you need to figure out how to pilot this thing.

It seems like the submarine can take a series of commands like `forward 1`, `down 2`, or `up 3`:

- `forward X` increases the horizontal position by `X` units.
- `down X` increases the depth by `X` units.
- `up X` decreases the depth by `X` units.

Note that since you're on a submarine, down and up affect your depth, and so they have the opposite result of what you might expect.

### Part One
The submarine seems to already have a planned course (your puzzle input). You should probably figure out where it's going. For example:

```forward 5
down 5
forward 8
up 3
down 8
forward 2
```

Your horizontal position and depth both start at `0`. The steps above would then modify them as follows:

- `forward 5` adds `5` to your horizontal position, a total of `5`.
- `down 5` adds `5` to your depth, resulting in a value of `5`.
- `forward 8` adds `8` to your horizontal position, a total of `13`.
- `up 3` decreases your depth by `3`, resulting in a value of `2`.
- `down 8` adds `8` to your depth, resulting in a value of `10`.
- `forward 2` adds `2` to your horizontal position, a total of `15`.

After following these instructions, you would have a horizontal position of `15` and a depth of `10`. (Multiplying these together produces `150`.)

Calculate the horizontal position and depth you would have after following the planned course. 

**What do you get if you multiply your final horizontal position by your final depth?**

### Part Two
Based on your calculations, the planned course doesn't seem to make any sense. You find the submarine manual and discover that the process is actually slightly more complicated.

In addition to horizontal position and depth, you'll also need to track a third value, **aim**, which also starts at `0`. The commands also mean something entirely different than you first thought:

- `down X` increases your aim by `X` units.
- `up X` decreases your aim by `X` units.
- `forward X` does two things:
    - It increases your horizontal position by `X` units.
    - It increases your depth by your aim multiplied by `X`.

Again note that since you're on a submarine, down and up do the opposite of what you might expect: "down" means aiming in the positive direction.

Now, the above example does something different:

- `forward 5` adds `5` to your horizontal position, a total of `5`. Because your aim is `0`, your depth does not change.
- `down 5` adds `5` to your aim, resulting in a value of `5`.
- `forward 8` adds `8` to your horizontal position, a total of `13`. Because your aim is `5`, your depth increases by `8*5=40`.
- `up 3` decreases your aim by `3`, resulting in a value of `2`.
- `down 8` adds `8` to your aim, resulting in a value of `10`.
- `forward 2` adds `2` to your horizontal position, a total of `15`. Because your aim is `10`, your depth increases by `2*10=20` to a total of `60`.

After following these new instructions, you would have a horizontal position of `15` and a depth of `60`. (Multiplying these produces `900`.)

Using this new interpretation of the commands, calculate the horizontal position and depth you would have after following the planned course. 

**What do you get if you multiply your final horizontal position by your final depth?**

## Solutions
The first thing I did was store the input data in a file and read it from the program. The data is then stored as an enumerable of `string` (direction) and `long` (distance) pair.

Using the enumerable of direction and distance pair, I used the GroupBy method to group the elements by direction (as key) with their corresponding distances.
```cs
_dirDistanceArray
    .GroupBy(x => x.direction, x => x.distance)
```

Continuing the chain, I then consolidate the values to a dictionary, using the direction as keys and getting the sum of all the distance values.
```cs
var sum = _dirDistanceArray
    .GroupBy(x => x.direction, x => x.distance)
    .ToDictionary(y => y.Key, y => y.Sum());
```

The problem needs the product of the final horizontal position (from the `forward` values) and the final depth (from the `down` and `up` values). To achieve this, I just used the dictionary we have created.

The final horizontal position is in the "forward" entry.
```cs
sum["forward"]
```

Meanwhile, the final depth is obtained by subtracting the overall `up` values from the overall `down` values.
```cs
(sum["down"] - sum["up"]
```

With these, the answer is the product of the horizontal position and the depth.
```cs
sum["forward"] * (sum["down"] - sum["up"]);
```
For part two, additional requirements involving aim is introduced. The first thing I did was to analyze the rules and list down the changes incurring in the following fields:
- horizontal distance
- depth
- aim

Let's review the rules one by one.
`down X` increases your aim by `X` units.
| Direction | Distance     | Depth             | Aim     |
|-----------|--------------|-------------------|---------|
| Down      | ~            | ~                 | Aim + x |

`up X` decreases your aim by `X` units.
| Direction | Distance     | Depth             | Aim     |
|-----------|--------------|-------------------|---------|
| Up        | ~            | ~                 | Aim - x |

`forward X` does two things:
- It increases your horizontal position by `X` units.
- It increases your depth by your aim multiplied by `X`.

| Direction | Distance     | Depth             | Aim     |
|-----------|--------------|-------------------|---------|
| Forward   | Distance + x | Depth + (Aim * x) | ~       |

For this one, I used the Aggregate method to apply an accumulator function to add up to the sums of the values of the three fields I mentioned below.

The values of distance, depth and aim are updated based on the current direction as the code iterates through the data.
```cs
(sum, curr) =>
    curr.direction switch
    {
        "up" => (sum.distance, sum.depth, sum.aim - curr.distance),
        "down" => (sum.distance, sum.depth, sum.aim + curr.distance),
        "forward" => (sum.distance + curr.distance, sum.depth + sum.aim * curr.distance, sum.aim),
        _ => throw new NotImplementedException()
    }
```
The result of the Aggregate method are the calculated sums of distance, depth and aims. With these, the answer is the product of the horizontal position and the depth.
```cs
calculated.distance * calculated.depth
```


## Notes
Continuing the LINQ train!
- [Enumerable.GroupBy](https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.groupby?view=net-6.0)
- [Enumerable.Aggregate](https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.aggregate?view=net-6.0)

I did not include my actual data in the project. Please see this [Reddit comment](https://www.reddit.com/r/adventofcode/comments/e7khy8/comment/fa13hb9/?utm_source=share&utm_medium=web2x&context=3) for more details.
> Please don't share your input anywhere as that makes it easier for unscrupulous folks to reverse-engineer all the hard work that [/u/topaz2078](https://www.reddit.com/user/topaz2078) has put into this event.