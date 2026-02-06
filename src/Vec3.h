#ifndef VEC3_H
#define VEC3_H

#include <iostream>
#include <vector>
#include <cmath>
#include <memory>
#include <unistd.h>

// 3D 0_o
class Vec3 {
public:
    float x, y, z;

    Vec3(float x = 0, float y = 0, float z = 0) : x(x), y(y), z(z) {}

    Vec3 operator+(const Vec3& v) const {
        return Vec3(x + v.x, y + v.y, z + v.z);
    }

    Vec3 operator-(const Vec3& v) const {
        return Vec3(x - v.x, y - v.y, z - v.z);
    }

    Vec3 operator*(float s) const {
        return Vec3(x * s, y * s, z * s);
    }

    Vec3 operator-() const {
        return Vec3(-x, -y, -z);
    }

    float scalar(const Vec3& v) const;
    Vec3 normalize() const;
};

class Ray {
public:
    Vec3 origin;
    Vec3 direction;

    Ray(const Vec3& o, const Vec3& d);
};

class IShape {
public:
    virtual ~IShape() {}
    virtual bool intersect(const Ray& ray, float& t) const = 0;	// ray touch surface
    virtual Vec3 normalAt(const Vec3& point) const = 0;			// normal out of shape
};

class Sphere : public IShape {
public:
    Vec3 center;
    float radius;

    Sphere(const Vec3& c, float r);

    bool intersect(const Ray& ray, float& t) const override;
    Vec3 normalAt(const Vec3& p) const override;
};

class Cube : public IShape {
public:
    Vec3 min;
    Vec3 max;

    Cube(const Vec3& minCorner, const Vec3& maxCorner);

    bool intersect(const Ray& ray, float& t) const override;
    Vec3 normalAt(const Vec3& p) const override;
};

class Camera {
public:
    Vec3 position;
    Vec3 target;
    float fov;

    Camera(float fov = 90.0f);
    Ray makeRay(float u, float v) const;
};

class Renderer {
public:
    int width, height;
    Camera camera;
    std::vector<std::shared_ptr<IShape>> objects;
    Vec3 lightDir;

    Renderer(int w, int h);
    char shade(float brightness) const;
    void render() const;
};

#endif /* VEC3_H_ */
