#include "Vec3.h"

// Vec3
float Vec3::scalar(const Vec3& v) const {			//vec3 direction check
    return x * v.x + y * v.y + z * v.z;
}

Vec3 Vec3::normalize() const {					//trim and normalize vectors. DONT REMOVE LIGHT BECOMES JUNKY
    float len = std::sqrt(scalar(*this));
    if (len < 0.0001f) return Vec3(0, 0, 0);
    return *this * (1.0f / len);
}

// Ray
Ray::Ray(const Vec3& o, const Vec3& d) : origin(o), direction(d.normalize()) {}

// Sphere
Sphere::Sphere(const Vec3& c, float r) : center(c), radius(r) {}

bool Sphere::intersect(const Ray& ray, float& t) const {
    Vec3 oc = ray.origin - center;	// shape relative to ray

    float a = ray.direction.scalar(ray.direction);	//how far?
    float b = 2.0f * oc.scalar(ray.direction);		//does it look at shape?
    float c = oc.scalar(oc) - radius * radius;		//how far from shape relative to its size

    float disc = b * b - 4 * a * c;		//can ray touch shape?
    if (disc <= 0) return false;

    float sqrtDisc = std::sqrt(disc);
    float t0 = (-b - sqrtDisc) / (2 * a);	//shape enter point
    float t1 = (-b + sqrtDisc) / (2 * a);	//shape exit point 

    t = t0;
    if (t < 0) t = t1;		//if we inside sphere and t0 is behind us, render from inside out

    return t > 0;
}

Vec3 Sphere::normalAt(const Vec3& p) const {
    return (p - center).normalize();
}

// Cube
Cube::Cube(const Vec3& minCorner, const Vec3& maxCorner) : min(minCorner), max(maxCorner) {}

bool Cube::intersect(const Ray& ray, float& t) const {
    float tMin = (min.x - ray.origin.x) / ray.direction.x;	//shapeX enter point
    float tMax = (max.x - ray.origin.x) / ray.direction.x;	//shapeX exit point
    if (tMin > tMax) std::swap(tMin, tMax);			//swap if wrong order

    float tyMin = (min.y - ray.origin.y) / ray.direction.y;	//shapeY enter point
    float tyMax = (max.y - ray.origin.y) / ray.direction.y;	//shapeY exit point
    if (tyMin > tyMax) std::swap(tyMin, tyMax);		//swap if wrong order

    if (tMin > tyMax || tyMin > tMax)
        return false;

    if (tyMin > tMin) tMin = tyMin;
    if (tyMax < tMax) tMax = tyMax;

    float tzMin = (min.z - ray.origin.z) / ray.direction.z;	//shapeZ enter point
    float tzMax = (max.z - ray.origin.z) / ray.direction.z;	//shapeZ exit point
    if (tzMin > tzMax) std::swap(tzMin, tzMax);		//swap if wrong order

    if (tMin > tzMax || tzMin > tMax)
        return false;

    t = tMin > 0 ? tMin : tMax;
    return t > 0;
}

Vec3 Cube::normalAt(const Vec3& p) const {
    const float epsilon = 0.001f;	//small

    if (std::abs(p.x - min.x) < epsilon) return Vec3(-1, 0, 0);
    if (std::abs(p.x - max.x) < epsilon) return Vec3(1, 0, 0);
    if (std::abs(p.y - min.y) < epsilon) return Vec3(0, -1, 0);
    if (std::abs(p.y - max.y) < epsilon) return Vec3(0, 1, 0);
    if (std::abs(p.z - min.z) < epsilon) return Vec3(0, 0, -1);

    return Vec3(0, 0, 1); //front(?) face fallback
}

// Camera
Camera::Camera(float fov) : target(0, 0, 0), fov(fov) {}

Ray Camera::makeRay(float u, float v) const {
    Vec3 forward = (target - position).normalize();

    Vec3 right = Vec3(forward.z, 0, -forward.x);
    Vec3 up = Vec3(
        right.y * forward.z - right.z * forward.y,
        right.z * forward.x - right.x * forward.z,
        right.x * forward.y - right.y * forward.x
    );

    Vec3 dir = forward + right * u + up * v;
    return Ray(position, dir);
}

// Renderer
Renderer::Renderer(int w, int h) : width(w), height(h), lightDir(Vec3(-1, 2, -1)) {}

char Renderer::shade(float brightness) const {
    const char* gradient = " .:-=+*%#";
    int idx = (int)(brightness * 8);
    if (idx < 0) idx = 0;
    if (idx > 8) idx = 8;
    return gradient[idx];
}

void Renderer::render() const {
    Vec3 normalizedLight = lightDir.normalize();

    for (int y = 0; y < height; y++) {
        for (int x = 0; x < width; x++) {

            float u = (2.0f * x / width - 1.0f) * (float(width) / height);
            float v = 1.0f - 2.0f * y / height;

            Ray ray = camera.makeRay(u, v);

            float closest = 999999.0f;
            const IShape* hit = nullptr;

            for (auto& obj : objects) {
                float t;
                if (obj->intersect(ray, t) && t < closest) {
                    closest = t;
                    hit = obj.get();	//get closest hit of a shape
                }
            }

            if (hit) {
                Vec3 hitPoint = ray.origin + ray.direction * closest;
                Vec3 normal = hit->normalAt(hitPoint);

                float ambient = 0.18f; // ambient light
                float diff = normal.scalar(-normalizedLight);

                if (diff < 0) diff = 0;	//diffusing some light

                float lightValue = ambient + diff * 0.82f;
                std::cout << shade(lightValue);

            } else {	// if nothing was hit, drawing background
                float y = ray.direction.y;

                if (y > 0.15f) {
                    std::cout << '.';     // sky
                } else if (y > 0.02f) {
                    std::cout << ':';     // near horizon
                } else if (y > -0.02f) {
                    std::cout << '=';     // horizon line
                } else if (y > -0.15f) {
                    std::cout << '-';     // near ground
                } else {
                    std::cout << '_';     // ground
                }
            }
        }
        std::cout << '\n';
    }
}
