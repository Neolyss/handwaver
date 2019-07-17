# Mathematical Audit of Algorithms for Scale and Dimension Scene

In this document, we describe the methods we use (and their sources) for computing the dynamic figures described in the scale and dimension scene.  We acknolwedge that the cases presented here may be limited in nature.  While this scene was being developed a team was working to integrate with a server-side implementation of GeoGebra.  Any generalized version would use the GGB implementation to do the heavy lifting.

## Cross-Sections



### Intersection of a plane and a circle

Consider a circle $C$ in a plane orthagonal to $(0,1,0)$ and and a plane orthagonal to $(0,0,1)$.
Let $R$ be the radius of the circle.
Let $\rho$ be the signed distance between the center of the circle and the line $L$ formed by the intersection of the plane and the circle's plane.
Let $2x$ be the distance between the points formed by the intersection of the circle and the line $L$.
Since there is reflective symmetry in any direction on the circle, the points lie $\plusminus x$ in the direction of $L$ from the center of the circle.
By the pythagorean theorem we have $$(x)^2 + \rho^2 = R^2$$

  So the points lie at $(\plusminus \sqrt{R^2 - \rho^2},0,\rho)$.  If $\rho = R$, the two values are equal and there is only one point formed by the intersection of $C$ and $L$.  If $\rho > R$, then there is no intersection.

[//]: # add a diagram to illustrate

```c#
//if cross section only hits the edge of the circle
if (math.abs(height) == radius)
{
    //if top of circle, create point at intersection
    if (height == radius)
    {
        segmentEndPoint0 = Vector3.up * radius;
    }

    //if bottom of circle, create point at intersection
    else if (height == -radius)
    {
        segmentEndPoint0 = Vector3.down * radius;
    }
}
//cross section is a line that hits two points on the circle (height smaller than radius of circle)
else if (math.abs(height) < radius)
{
    //horizontal distance from center of circle to point on line segment
    float segmentLength = Mathf.Sqrt(1f - Mathf.Pow(height, 2));

    //calculations for endpoint coordinates of line segment
    segmentEndPoint0 = (Vector3.up * height) + (Vector3.left * segmentLength);
    segmentEndPoint1 = (Vector3.up * height) + (Vector3.right * segmentLength);
}
//height for cross section is outside of circle 
else if (math.abs(height) > radius)
{
    Debug.Log("Height is out of range of object.");
}
```

### Intersection of a plane and an annulus

Consider an annulus $A$ with inner radius $r$ and outer radius $R$. Let $\rho$ be the distance between the center of the annulus and the line segment $L$ formed by the intersection of a plane with the annulus. This intersection will yield either a point, a line segment, or two line segments. 
Let $2x$ be the distance between the two points in the case where the intersection yields one line segment, which will occur when the intersection is in between $R$ and $r$. By the same logic as the cross-section of a circle, we have the equation 
$$(x)^2 + \rho^2 = R^2$$

Following the logic used for the circle, the points lie at $(\plusminus \sqrt{R^2 - \rho^2},0,\rho)$.  If $\rho = R$, the two values are equal and there is only one point formed by the intersection of $C$ and $L$. 

The other case results from the intersection occurcing at a height with a magnitude less than that of the inner radius, giving two line segments. The math for this case is essentially the same as one line segment, but with an additional calculation for the two inner points. 
Let $2x$ be the distance between the two outer points, one for each line segment. As before, their location can be calculated by $$(x)^2 + \rho^2 = R^2$$

Thus, the points lie at $(\plusminus \sqrt{R^2 - \rho^2},0,\rho)$. 
Now let $2w$ be the distance between the two inner points. The location for both of those points can be found using the same calculation as the outer points, but using $r$ and $w$ instead of $R$ and $x$. Thus, the equation is $$(w)^2 + \rho^2 = r^2$$

Thus, the inner points lie at $(\plusminus \sqrt{r^2 - \rho^2},0,\rho)$. In the case that $\rho$ > $R$, there is no intersection between the annulus and plane.

```c#
//cross-section only hits edge of annulus
if (math.abs(height) == outerRadius)
{
//if top edge, create point at intersection
if (height == outerRadius)
{
    segmentAEndPoint0 = Vector3.up * outerRadius;
}
//if bottom edge, create point at intersection
else
{
    segmentAEndPoint0 = Vector3.down * outerRadius;
}
}
//cross section is a line segment in between the inner circle and outer circle
else if (math.abs(height) < outerRadius && math.abs(height) >= innerRadius)
{
//horizontal distance from center to point on outer edge of annulus
x1 = (Mathf.Sqrt(Mathf.Pow(outerRadius, 2) - Mathf.Pow(height, 2)));

//calculations for coordinates of line segment endpoints
segmentAEndPoint0 = (Vector3.up * height) + (Vector3.right * (x1));
segmentAEndPoint1 = (Vector3.up * height) + (Vector3.left * (x1));
}
//cross section height is less than the inner radius, resulting in two line segments
else if (math.abs(height) < innerRadius)
{
//horizontal distance from center to point on outer edge (x1) and inner edge (x2) of annulus
x1 = (Mathf.Sqrt(Mathf.Pow(outerRadius, 2) - Mathf.Pow(height, 2)));
x2 = (Mathf.Sqrt(Mathf.Pow(innerRadius, 2) - Mathf.Pow(height, 2)));

//calculations for inner and outer endpoints for each line segment
segmentAEndPoint0 = (Vector3.up * height) + (Vector3.left * (x1));
segmentAEndPoint1 = (Vector3.up * height) + (Vector3.left * (x2));

segmentBEndPoint0 = (Vector3.up * height) + (Vector3.right * (x2));
segmentBEndPoint1 = (Vector3.up * height) + (Vector3.right * (x1));
}
//cross section height is out of range of annulus
else if (math.abs(height) > outerRadius)
{
Debug.Log("Height is out of range of object.");
}
```

### Intersection of a plane and a sphere

Consider a sphere $S$ with radius $r$ centered at the origin. Let $\rho$ be the distance between the center of the sphere and the plane $P$ formed by intersecting the sphere with a plane. In the event that the intersection only hits the top or bottom edge of the sphere, the resulting cross-section will simply be a point at the top or bottom of the sphere. 

```c#
//if cross section only hits the edge of the circle
if (math.abs(height) == radius)
{
    //if top of sphere, create point at intersection
    if (height == radius)
    {
        Vector3 segmentEndPoint0 = Vector3.up * radius;
    }

    //if bottom of circle, create point at intersection
    else if (height == -radius)
    {
        Vector3 segmentEndPoint0 = Vector3.down * radius;
    }

}
//cross section is a circle
else if (math.abs(height) < radius)
{
    //horizontal distance from center of circle to point on line segment
    renderCircle(Mathf.Sqrt(Mathf.Pow(radius,2) - Mathf.Pow(height, 2)), height*Vector3.up);
}
//height for cross section is outside of circle 
else if (math.abs(height) > radius)
{
    Debug.Log("Height is out of range of object.");
}
```

### Intersection of a plane and a torus

``` c#

```

### Intersection of a hyperplane and a hypersphere

``` c#

```

### Intersection of a hyperplane and a three-torus

``` c#

```


## Nets

### Equaliaterial Triangle

The net of a triangle is three congruant line segments.  In it's unfolded state, the line segments are colinear. To fold the triangle net, hold one segment fixed and rotate the other two segments (clockwise and counterclockwise, respectively) by $\frac{2}{3} \pi$ radians.

```C#
//angle of rotation in degrees (Unity.Mathematics works in degrees)
float t = percentFolded * 120f;
//matrix of vertices 
Vector3[] result = new Vector3[4];
//initial vertices
result[2] = Vector3.zero;
result[1] = Vector3.right;
//rotate vertex by t or -t around (0, 1, 0) with appropriate vector manipulation to connect triangle
result[0] = result[1] + Quaternion.AngleAxis(t, Vector3.up) * Vector3.right;
result[3] = result[2] + Quaternion.AngleAxis(-t, Vector3.up) * Vector3.left;
```

### Square

The net of a square is four congruant line segments.  In it's unfolded state, the line segments are colinear.  To fold the square net, hold one of the middle segments fixed.  Rotate the two adjacent segments around their respective endpoints by $\frac{\pi}{4}$ radians. The remaining segment is adjacent to one of the rotated segments (segment A).  Rotate that segment by ninety degrees around it's joining endpoint, with respect to the direction of segment A.  In effect, this vertex is rotated by $\frac{\pi}{2}$ with respect to it's origional direction.

```c#
//angle of rotation in degrees (Unity.Mathematics works in degrees)
float angle = percentFolded * 90f;
//matrix of vertices
Vector3[] result = new Vector3[5];
//initial vertices that don't need to move/are pivot points
result[2] = Vector3.zero;
result[1] = Vector3.right;
//rotate vertice by t or -t around (0, 1, 0) 
result[0] = result[1] + Quaternion.AngleAxis(angle, Vector3.up) * Vector3.right;
result[3] = result[2] + Quaternion.AngleAxis(-angle, Vector3.up) * Vector3.left;
//rotate vertice by -2t around (0, 1, 0)
result[4] = result[3] + Quaternion.AngleAxis(-2 * angle, Vector3.up) * Vector3.left;
```

### Cube

The net of a cube is a collection of six congruant squares,
One square remains fixed in the center.  
Four squares share an edge with the center square, and rotate around that edge by $\frac{\pi}{4}$ to fold up the net.
On one of those four squares, a final square is constructed sharing the opposite edge.
The final square is rotated by $\frac{\pi}{4}$ with respect to the adjacent square, or $\frac{\pi}{2}$ with respect to it's origional orientation, around it's shared edge.
```c        
float degreeFolded = percentFolded * 90f + 180f;
//14 points on cube net
Vector3[] result = new Vector3[14];

//4 vertices for base of cube
result[0] = .5f * (Vector3.forward + Vector3.right);
result[1] = .5f * (Vector3.forward + Vector3.left);
result[2] = .5f * (Vector3.back + Vector3.left);
result[3] = .5f * (Vector3.back + Vector3.right);

//use squareVert() to fold outer squares up relative to base square 
result[4] = squareVert(result[3], result[0], result[1], degreeFolded);
result[5] = squareVert(result[3], result[0], result[2], degreeFolded);

result[6] = squareVert(result[0], result[1], result[3], degreeFolded);
result[7] = squareVert(result[0], result[1], result[2], degreeFolded);


result[8] = squareVert(result[2], result[3], result[1], degreeFolded);
result[9] = squareVert(result[2], result[3], result[0], degreeFolded);

result[10] = squareVert(result[1], result[2], result[0], degreeFolded);
result[11] = squareVert(result[1], result[2], result[3], degreeFolded);

result[12] = squareVert(result[10], result[11], result[1], degreeFolded);
result[13] = squareVert(result[10], result[11], result[2], degreeFolded);
```

```c#
private static Vector3 squareVert(Vector3 nSegmentA, Vector3 nSegmentB, Vector3 oppositePoint,
float degreeFolded)
{
    return Quaternion.AngleAxis(degreeFolded, (nSegmentA - nSegmentB).normalized) *
       (oppositePoint - (nSegmentA + nSegmentB) / 2f) + (nSegmentA + nSegmentB) / 2f;
}
```

### Regular Tetrahedron

The net of a tetrahedron is a collection of four congruant equilaterial triangles.  One traingle remains fixed in the center, and each of the remaining triangles shares an edge with the center triangle.  All triangles except the center rotate around their shared edge by $\arccos\frac{1}{3}$.

```c#
//scale the degree folded by the diehdral angle of the folded tetrahedron of ~70.52
float degreefolded = percentfolded * COMPLETEDFOLD + 180f;
//6 vertices on tetrahedron
Vector3[] result = new Vector3[6];

//inner 3 vertices
result[0] = Vector3.right * (Mathf.Sqrt(3f) / 2f) + Vector3.forward * .5f;
result[1] = Vector3.right * (Mathf.Sqrt(3f) / 2f) + Vector3.back * .5f;
result[2] = Vector3.zero;

//vertex between 0 and 1
//use trivert() to fold outer vertices up relative to inner vertices
result[3] = triVert(result[0], result[1], result[2], degreefolded);

//vertex between 1 and 2
result[4] = triVert(result[1], result[2], result[0], degreefolded);
//vertex between 0 and 2
result[5] = triVert(result[2], result[0], result[1], degreefolded);
```

```c#
private static Vector3 triVert(Vector3 nSegmentA, Vector3 nSegmentB, Vector3 oppositePoint, float degreeFolded)
{
    return Quaternion.AngleAxis(degreeFolded, (nSegmentA - nSegmentB).normalized) *
           (oppositePoint - (nSegmentA + nSegmentB) / 2f) + (nSegmentA + nSegmentB) / 2f;
}
```

### 5-cell

```c#
//8 points on unfolded fivecell
float4[] result = new float4[8];

//core tetrahedron (does not fold)
//coordiantes from wikipedia  https://en.wikipedia.org/wiki/5-cell, centered at origin, 
result[0] = (new float4(1f / math.sqrt(10f), 1f / math.sqrt(6f), 1f / math.sqrt(3f), 1f)) / 2f;
result[1] = (new float4(1f / math.sqrt(10f), 1f / math.sqrt(6f), 1f / math.sqrt(3f), -1f)) / 2f;
result[2] = (new float4(1f / math.sqrt(10f), 1f / math.sqrt(6f), -2f / math.sqrt(3f), 0f)) / 2f;
result[3] = new float4(1f / math.sqrt(10f), -math.sqrt(3f / 2f), 0f, 0f);

//find position of convergent point for other tetrahedrons in the net.
float4 apex = new float4(-2 * math.sqrt(2f / 5f), 0f, 0f, 0f);
//TODO consider making the initial projection onto n

//apex of tetrahedron for each additional tetrahedron(from fases of first) foldling by degree t
float4 center1 = (result[0] + result[1] + result[2]) / 3f;
float4 dir1 = center1 - result[3];
result[4] = center1 + Math.Operations.rotate(dir1, dir1, apex - center1, degreeFolded);

float4 center2 = (result[0] + result[2] + result[3]) / 3f;
float4 dir2 = center2 - result[1];
result[5] = center2 + Math.Operations.rotate(dir2, dir2, apex - center2, degreeFolded);

float4 center3 = (result[0] + result[1] + result[3]) / 3f;
float4 dir3 = center3 - result[2];
result[6] = center3 + Math.Operations.rotate(dir3, dir3, apex - center3, degreeFolded);

float4 center4 = (result[1] + result[2] + result[3]) / 3f;
float4 dir4 = center4 - result[0];
result[7] = center4 + Math.Operations.rotate(dir4, dir4, apex - center4, degreeFolded);
```

### 8-cell


```c#
 //core cube (does not fold)
result[0] = (up + right + forward)/2f;
result[1] = (up + left + forward)/2f;
result[2] = (up + left + back)/2f;
result[3] = (up + right + back)/2f;

result[4] = (down + right + forward) / 2f;
result[5] = (down + left + forward) / 2f;
result[6] = (down + left + back) / 2f;
result[7] = (down + right + back) / 2f;

//above up face.
result[8] = result[0] + Math.Operations.rotate(up, up, wForward, degreeFolded);
result[9] = result[1] + Math.Operations.rotate(up,up, wForward, degreeFolded);
result[10] = result[2] + Math.Operations.rotate(up, up, wForward, degreeFolded);
result[11] = result[3] + Math.Operations.rotate(up, up, wForward, degreeFolded);

//below down face
result[12] = result[4] + Math.Operations.rotate(down,down, wForward, degreeFolded);
result[13] = result[5] + Math.Operations.rotate(down,down, wForward, degreeFolded);
result[14] = result[6] + Math.Operations.rotate(down,down, wForward, degreeFolded);
result[15] = result[7] + Math.Operations.rotate(down,down, wForward, degreeFolded);

//right of right face;
result[16] = result[0] + Math.Operations.rotate(right, right, wForward, degreeFolded);
result[17] = result[3] + Math.Operations.rotate(right, right, wForward, degreeFolded);
result[18] = result[7] + Math.Operations.rotate(right, right, wForward, degreeFolded);
result[19] = result[4] + Math.Operations.rotate(right, right, wForward, degreeFolded);

//left of left face
result[20] = result[1] + Math.Operations.rotate(left,left, wForward, degreeFolded);
result[21] = result[2] + Math.Operations.rotate(left, left,wForward, degreeFolded);
result[22] = result[6] + Math.Operations.rotate(left, left,wForward, degreeFolded);
result[23] = result[5] + Math.Operations.rotate(left, left,wForward, degreeFolded);

//forward of forward face.
result[24] = result[0] + Math.Operations.rotate(forward,forward, wForward, degreeFolded);
result[25] = result[1] + Math.Operations.rotate(forward,forward, wForward, degreeFolded);
result[26] = result[5] + Math.Operations.rotate(forward,forward, wForward, degreeFolded);
result[27] = result[4] + Math.Operations.rotate(forward, forward,wForward, degreeFolded);

//back of back face.
result[28] = result[2] + Math.Operations.rotate(back,back, wForward, degreeFolded);
result[29] = result[3] + Math.Operations.rotate(back,back, wForward, degreeFolded);
result[30] = result[7] + Math.Operations.rotate(back,back, wForward, degreeFolded);
result[31] = result[6] + Math.Operations.rotate(back,back, wForward, degreeFolded);

float4 tmp = Math.Operations.rotate(down,down, wForward, degreeFolded);
//down of double down.
result[32] = result[12] + Math.Operations.rotate(tmp,tmp, wForward, degreeFolded);
result[33] = result[13] + Math.Operations.rotate(tmp,tmp, wForward, degreeFolded);
result[34] = result[14] + Math.Operations.rotate(tmp,tmp, wForward, degreeFolded);
result[35] = result[15] + Math.Operations.rotate(tmp,tmp, wForward, degreeFolded);
```

## Orthogrpahic Projection from 4D to 3D


## References