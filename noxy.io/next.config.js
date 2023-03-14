/**
 * @type {import("next").NextConfig}
 * @see [configuration documentation](https://nextjs.org/docs/api-reference/next.config.js/introduction)
 * @see [configuration github](https://github.com/vercel/next.js/blob/canary/packages/next/server/config-shared.ts#L184)
 */
const nextConfig = {
  reactStrictMode: false,
  async headers() {
    return [
      {
        // matching all API routes
        source: "/api/:path*",
        headers: [
          {key: "Access-Control-Allow-Credentials", value: "true"},
          {key: "Access-Control-Allow-Origin", value: "*"},
          {key: "Access-Control-Allow-Methods", value: "GET,OPTIONS,PATCH,DELETE,POST,PUT"},
          {key: "Access-Control-Allow-Headers", value: "X-CSRF-Token, X-Requested-With, Accept, Accept-Version, Content-Length, Content-MD5, Content-Type, Date, X-Api-Version"},
        ],
      },
    ];
  },
};

module.exports = nextConfig;
