# Create Toolpad App

This is a [Next.js](https://nextjs.org/) project bootstrapped with [`create-toolpad-app`](https://github.com/vercel/next.js/tree/canary/packages/create-next-app).

## Setup

Add the `NEXT_PUBLIC_API_URL` and `NODE_TLS_REJECT_UNAUTHORIZED` to the .env.local file:
NEXT_PUBLIC_API_URL=https://localhost:7203
NODE_TLS_REJECT_UNAUTHORIZED=0

## Getting Started

1. Run command to install node_modules: `yarn install`
2. Run command to start dev server: `yarn dev`

Open [http://localhost:3000] with your browser to see the result.
You BE should be also running on port [https://localhost:7203/swagger]

## Deploy on Vercel

1. Run command: `yarn build`
2. Run command if you want to test local build: `yarn start`
3. Copy artefacts `.next` folder to destination server or cloud
