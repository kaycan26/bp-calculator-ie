import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
    stages: [
        { duration: '10s', target: 10 }, // ramp to 10 VUs
        { duration: '10s', target: 20 }, // ramp to 20 VUs
        { duration: '10s', target: 0 },  // ramp down
    ],
    thresholds: {
        http_req_duration: ['p(95)<500'],   // 95% under 500ms
        http_req_failed: ['rate<0.01'],      // <1% errors
    },
};

export default function () {
    const res = http.get('http://localhost:5000/');

    check(res, {
        'status is 200': (r) => r.status === 200,
    });

    sleep(1);
}
