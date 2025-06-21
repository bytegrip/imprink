'use client';

import { useState } from 'react';
import { loadStripe } from '@stripe/stripe-js';
import { Elements } from '@stripe/react-stripe-js';
import PaymentForm from './components/PaymentForm';
import './globals.css';

const stripePromise = loadStripe('pk_test_51RaxJBRrcXIyofFGYIfUxzWTLPBfr1A0f2VBjo0lOjHfTBtyVpJKBjVUJ972p5AytGl4LBrgQccwHkp6EYu4liln00vEAf2D4e');

const products = [
    { id: '1', name: 'Premium Widget', price: 2999, description: 'High-quality widget for professionals' },
    { id: '2', name: 'Standard Widget', price: 1999, description: 'Reliable widget for everyday use' },
    { id: '3', name: 'Basic Widget', price: 999, description: 'Entry-level widget for beginners' }
];

export default function Home() {
    const [selectedProduct, setSelectedProduct] = useState(null);
    const [clientSecret, setClientSecret] = useState('');
    const [orderId, setOrderId] = useState('');
    const [loading, setLoading] = useState(false);

    const handleProductSelect = async (product) => {
        setLoading(true);
        setSelectedProduct(product);

        const newOrderId = Math.floor(Math.random() * 10000).toString();
        setOrderId(newOrderId);

        try {
            const response = await fetch('https://impr.ink/api/stripe/create-payment-intent', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    amount: product.price,
                    orderId: newOrderId
                }),
            });

            const data = await response.json();

            if (data.clientSecret) {
                setClientSecret(data.clientSecret);
            } else {
                console.error('Error creating payment intent:', data.error);
            }
        } catch (error) {
            console.error('Error:', error);
        } finally {
            setLoading(false);
        }
    };

    const handlePaymentSuccess = () => {
        setSelectedProduct(null);
        setClientSecret('');
        setOrderId('');
    };

    const handleBackToProducts = () => {
        setSelectedProduct(null);
        setClientSecret('');
        setOrderId('');
    };

    const appearance = {
        theme: 'stripe',
        variables: {
            colorPrimary: '#0570de',
            colorBackground: '#ffffff',
            colorText: '#30313d',
            colorDanger: '#df1b41',
            fontFamily: 'Ideal Sans, system-ui, sans-serif',
            spacingUnit: '2px',
            borderRadius: '4px',
        },
    };

    const options = {
        clientSecret,
        appearance,
    };

    return (
        <div className="container">
            <header>
                <h1>🛍️ Stripe Payment Demo</h1>
                <p>Select a product to purchase</p>
            </header>

            {!selectedProduct ? (
                <div className="products">
                    <h2>Products</h2>
                    <div className="product-grid">
                        {products.map((product) => (
                            <div key={product.id} className="product-card">
                                <h3>{product.name}</h3>
                                <p className="description">{product.description}</p>
                                <p className="price">${(product.price / 100).toFixed(2)}</p>
                                <button
                                    onClick={() => handleProductSelect(product)}
                                    disabled={loading}
                                    className="select-btn"
                                >
                                    {loading ? 'Loading...' : 'Select'}
                                </button>
                            </div>
                        ))}
                    </div>
                </div>
            ) : (
                <div className="checkout">
                    <div className="order-summary">
                        <h2>Order Summary</h2>
                        <div className="order-details">
                            <p><strong>Product:</strong> {selectedProduct.name}</p>
                            <p><strong>Order ID:</strong> {orderId}</p>
                            <p><strong>Amount:</strong> ${(selectedProduct.price / 100).toFixed(2)}</p>
                        </div>
                    </div>

                    {clientSecret && (
                        <Elements options={options} stripe={stripePromise}>
                            <PaymentForm
                                onSuccess={handlePaymentSuccess}
                                orderId={orderId}
                            />
                        </Elements>
                    )}

                    <button
                        onClick={handleBackToProducts}
                        className="back-btn"
                    >
                        ← Back to Products
                    </button>
                </div>
            )}
        </div>
    );
}