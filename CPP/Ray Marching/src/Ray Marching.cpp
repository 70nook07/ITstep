#include "Vec3.h"
#include <cmath>

int main() {
    const int width = 100;
    const int height = 40;

    Renderer renderer(width, height);

    renderer.objects.push_back(
        std::make_shared<Sphere>(Vec3(0, 1, 0), 1.4f)
    );

    renderer.objects.push_back(
        std::make_shared<Sphere>(Vec3(2, 1, 2), 0.4f)
    );

    renderer.objects.push_back(
        std::make_shared<Cube>(
            Vec3(1.5f, -1.0f, -1.0f),
            Vec3(2.5f, 0.0f, 1.0f)
        )
    );

    float angle = 0.0f;

    for (int frame = 0; frame > -1; frame++) {
        angle += 0.05f;

        renderer.camera.target = Vec3(0, 0, 0); //looking at the center
        renderer.camera.position = Vec3(	// rotating the cam at the scene
            std::cos(angle) * 4.0f,
            -2.0f,
            std::sin(angle) * 4.0f
        );

        std::cout << "\033[2J"; //clear console
        renderer.render();

        usleep(200000); // 0.2 sec nap
    }

    return 0;
}
