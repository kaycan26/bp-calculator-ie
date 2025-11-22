import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
    vus: 5,          // 5 virtual users
    duration: '30s', // run for 30 seconds
};

export default function () {
    // APP_BASE_URL is passed from GitHub Actions as an environment variable
    const res = http.get(`${__ENV.APP_BASE_URL}/`);

    check(res, {
        'status is 200': (r) => r.status === 200,
        'page has BP form': (r) => r.body.includes('BP Category Calculator'),
        'page loads under 500ms': (r) => r.timings.duration < 500,
    });

    sleep(1);
}
